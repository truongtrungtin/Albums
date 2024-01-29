import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NavBarComponent } from './nav-bar/nav-bar.component';
import { RouterModule } from '@angular/router';
import { NotFoundComponent } from './not-found/not-found.component';
import { ServerErrorComponent } from './server-error/server-error.component';
import { ToastrModule } from 'ngx-toastr';
import { ConnectionRefusedComponent } from './connection-refused/connection-refused.component';
import { SectionHeaderComponent } from './section-header/section-header.component';
import { BreadcrumbModule } from 'xng-breadcrumb';
import { UnauthorizedComponent } from './unauthorized/unauthorized.component';



@NgModule({
  declarations: [
    NavBarComponent,
    NotFoundComponent,
    ServerErrorComponent,
    ConnectionRefusedComponent,
    UnauthorizedComponent,
    SectionHeaderComponent
  ],
  imports: [
    CommonModule,
    RouterModule,
    ToastrModule.forRoot({
      positionClass: 'toast-bottom-right', 
      preventDuplicates: true,
    }),
    BreadcrumbModule,
  ],
  exports: [
    NavBarComponent,
    NotFoundComponent,
    ServerErrorComponent,
    ConnectionRefusedComponent,
    UnauthorizedComponent,
    SectionHeaderComponent
  ]
})
export class CoreModule { }
