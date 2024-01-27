// storeParams.model.ts

import { Product } from './Product';
import { Brand } from './brand';
import { Type } from './type';

export class StoreParams {
  sort: string  = 'NameAsc';
  search?: string | null;
  title: string | null = '';
  location: string | null = '';
  url: string | null = '';
  duringTime?: string | null;
  MonthCreate?: string | null;
  YearCreate?: string | null;
  name: string | null = '';
  
  productBrandId?: number | null;
  productTypeId?: number | null;
  products: Product[] = [];
  brands: Brand[] = [];
  types: Type[]= [];

  selectedBrand: Brand | null = {id:0, name:"All"}; // Default brand selection
  selectedType: Type | null = {id:0, name:"All"}; // Default type selection

  page: number = 1 ;
  pageSize: number = 9;
  totalPages = 0;
  totalItems = 0; 
}
