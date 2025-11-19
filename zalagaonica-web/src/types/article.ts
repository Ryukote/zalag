// types/article.ts - Prošireni tipovi

export interface Article {
  id: string;
  name: string;
  description: string;
  purchasePrice: number;
  retailPrice: number;
  salePrice?: number;
  taxRate: number;
  stock: number;
  unitOfMeasureCode: string;
  supplierName?: string;
  group?: string;
  status: string;
  warehouseType: string;
  saleInfoPrice?: number;
  saleInfoDate?: string;
  saleInfoCustomerName?: string;
  saleInfoCustomerId?: string;
  createdAt: string;
  updatedAt: string;
  warehouseId?: string;
  unitOfMeasureId?: string;
}


// types/sale.ts ili u article.ts ako preferiraš

export interface SaleData {
  id?: string;
  articleId: string;
  quantity: number;
  pricePerUnit: number;
  totalPrice: number;
  salePrice: number;
  saleDate: string;
  customerName: string;
  customerId?: string;
}


export interface Customer {
  id: string;
  name: string;
  email?: string;
  phone?: string;
}