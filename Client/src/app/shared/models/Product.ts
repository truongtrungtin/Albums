import { Time } from "@angular/common";
import { LocationImage } from "./locationImage";

export interface Product {
  productId: string;
  name: string;
  price: number;
  size: number;
  description: string;
  pictureUrl: string;
  productType: string;
  productBrand: string;
  duringTime: Time | null;
  url: string;
  MonthCreate?: number;
  YearCreate?: number;
}
