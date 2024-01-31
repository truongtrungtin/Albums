import { LocationImage } from "./locationImage";

export interface CreateProduct{
    name: string;
    productType: string;
    locationImage: LocationImage;
    file: File
}
