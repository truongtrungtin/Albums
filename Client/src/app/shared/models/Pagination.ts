
export interface Pagination<T> {
    pageIndex: number;
    totalPages: number;
    pageSize: number;
    totalItems: number;
    data: T[];
  }