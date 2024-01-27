// import { createId } from "@paralleldrive/cuid2"
import { Brand } from "./brand"
import { Type } from "./type"

export interface Basket{
    id: string
    items: BasketItem[]
}

export interface BasketItem{
    id: string
    productName: string
    price: number
    quantity: number
    pictureUrl: string
    productBrand: string
    productType: string

}

export class Basket implements Basket{
    // id = createId();
    items: BasketItem[] = []
}

export interface BasketTotal{
    shipping: number;
    subtotal: number;
    total: number;
}