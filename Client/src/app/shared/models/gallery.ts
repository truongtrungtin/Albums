import { Brand } from "./brand";
import { Type } from "./type";

export interface gallery {
  id: number;
  name: string;
  description: string;
  location: string;
  type: string;
  duringTime: boolean;
  url: string;
  createTime: string;
  size: number;
}