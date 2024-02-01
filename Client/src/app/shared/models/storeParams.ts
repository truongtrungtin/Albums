// storeParams.model.ts

import { Catalog } from './Catalog';
import { FileAttachment } from './FileAttachment';
import { Profile } from './Profile';

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
  
  fileExtention?: string | null;
  fileType?: string | null;
  fileAttachments: FileAttachment[] = [];
  fileExtentions: Catalog[] = [];
  fileTypes: Catalog[]= [];
  profiles: Profile[]= [];


  selectedFileType: Catalog | null = {catalogCode: "", catalogText_vi: "All" }; // Default type selection
  selectedFileExtention: Catalog | null = {catalogCode: "", catalogText_vi: "All",  }; // Default brand selection
  selectedProfile: Profile | null = {profileCode: "", profileName: "Choose Profile", profileId: "", avatar: "", createBy: ""}; // Default type selection

  page: number = 1 ;
  pageSize: number = 12;
  totalPages = 0;
  totalItems = 0; 
}
