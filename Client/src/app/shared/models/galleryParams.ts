// storeParams.model.ts

import { Type } from "./type";

export class galleryParams {
  sort: string  = 'NameAsc';

  title: string | null = '';
  location: string | null = '';
  url: string | null = '';
  duringTime?: string | null;
  MonthCreate?: number | null;
  YearCreate?: number | null;

  typeId?: number | null;
  types: Type[] = [];
  selectedType: Type | null = { id: 0, name: "All" }; // Default type selection

  page: number = 1;
  pageSize: number = 9;
  totalPages = 0;
  totalItems = 0; 
}
