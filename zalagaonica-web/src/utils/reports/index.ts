// Export all report generators

// Purchase Receipt
export { generatePurchaseReceipt } from './purchaseReceipt';
export type { PurchaseReceiptData } from './purchaseReceipt';
export { generatePurchaseReceiptReact } from './purchaseReceiptReactPDF';
export type { PurchaseReceiptDataReact } from './purchaseReceiptReactPDF';

// Pledge Agreement
export { generatePledgeAgreement } from './pledgeAgreement';
export type { PledgeAgreementData } from './pledgeAgreement';
export { generatePledgeAgreementReact } from './pledgeAgreementReactPDF';
export type { PledgeAgreementDataReact } from './pledgeAgreementReactPDF';

// Appraisal Request
export { generateAppraisalRequest } from './appraisalRequest';
export type { AppraisalRequestData } from './appraisalRequest';
export { generateAppraisalRequestReact } from './appraisalRequestReactPDF';
export type { AppraisalRequestDataReact } from './appraisalRequestReactPDF';

// Reservation Receipt
export { generateReservationReceipt } from './reservationReceipt';
export type { ReservationReceiptData } from './reservationReceipt';
export { generateReservationReceiptReact } from './reservationReceiptReactPDF';
export type { ReservationReceiptDataReact } from './reservationReceiptReactPDF';

// Inbound Calculation
export { generateInboundCalculation } from './inboundCalculation';
export type { InboundCalculationData, InboundCalculationItem } from './inboundCalculation';
export { generateInboundCalculationReact } from './inboundCalculationReactPDF';
export type { InboundCalculationDataReact } from './inboundCalculationReactPDF';

// Warehouse Transfer
export { generateWarehouseTransfer } from './warehouseTransfer';
export type { WarehouseTransferData, WarehouseTransferItem } from './warehouseTransfer';
export { generateWarehouseTransferReact } from './warehouseTransferReactPDF';
export type { WarehouseTransferDataReact } from './warehouseTransferReactPDF';
