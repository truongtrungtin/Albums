import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, NavigationEnd } from '@angular/router';
import { filter } from 'rxjs/operators';

@Component({
  selector: 'app-section-header',
  templateUrl: './section-header.component.html',
  styleUrls: ['./section-header.component.scss']
})
export class SectionHeaderComponent {
  breadcrumbs: Array<{ label: string, url: string }> = [];
  showHeader: boolean = true; // Add a flag to control header visibility

  constructor(
    private router: Router,
    private route: ActivatedRoute
  ) {
    this.router.events.pipe(
      filter(event => event instanceof NavigationEnd)
    ).subscribe(() => {
      this.checkCurrentRoute();
    });
  }

  // Inside your Component class
  excludedRoutes: string[] = ['/basket', '/checkout/address', '/', '/account/login', '/account/register'];

  checkCurrentRoute(): void {
    // Get the current route
    const currentRoute = this.router.url;

    // Update the visibility flag based on the current route
    this.showHeader = !this.excludedRoutes.includes(currentRoute);

    // If not one of the excluded routes, update breadcrumbs as usual
    if (this.showHeader) {
      // Get the root route (the route without any child routes)
      const rootRoute = this.route.root;

      // Traverse the route tree to build breadcrumbs
      this.breadcrumbs = this.createBreadcrumbs(rootRoute);
    }
  }

  
  createBreadcrumbs(route: ActivatedRoute, url: string = '', breadcrumbs: Array<{ label: string, url: string }> = []): Array<{ label: string, url: string }> {
    // Get the children of the route
    const children: ActivatedRoute[] = route.children;
  
    // If there are no more child routes, return the breadcrumbs
    if (children.length === 0) {
      return breadcrumbs;
    }
  
    // Iterate over the child routes
    for (const child of children) {
      // Get the route's data
      const routeURL: string = child.snapshot.url.map(segment => segment.path).join('/');
      if (routeURL !== '') {
        url += `/${routeURL}`;
      }
  
      // Add breadcrumb to the array
      breadcrumbs.push({ label: routeURL, url: url });
  
      // Recursively call createBreadcrumbs for the next level
      return this.createBreadcrumbs(child, url, breadcrumbs);
    }
  
    return breadcrumbs;
  }
}
