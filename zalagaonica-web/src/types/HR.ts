export interface Employee {
  id: string;
  fullName: string;
  position?: string;
  email?: string;
  phone?: string;
  hiredDate: string;
  isActive: boolean;
}

export enum VacationStatus {
  Pending = 0,
  Approved = 1,
  Rejected = 2
}

export enum VacationType {
  AnnualLeave = 0,    // godišnji odmor
  SickLeave = 1,      // bolovanje
  PaidLeave = 2       // plaćeni dopust
}

export interface Vacation {
  id: string;
  employeeId: string;
  employeeName: string;
  startDate: string;
  endDate: string;
  status: VacationStatus;
  type: VacationType;
  requestDate: string;
  reason?: string;
  approvedBy?: string;
  approvedDate?: string;
  rejectionReason?: string;
  totalDays: number;
}

// Helper functions to convert between enums and display strings
export const getVacationStatusText = (status: VacationStatus): string => {
  switch (status) {
    case VacationStatus.Pending: return 'na čekanju';
    case VacationStatus.Approved: return 'odobreno';
    case VacationStatus.Rejected: return 'odbijeno';
    default: return 'nepoznato';
  }
};

export const getVacationTypeText = (type: VacationType): string => {
  switch (type) {
    case VacationType.AnnualLeave: return 'godišnji odmor';
    case VacationType.SickLeave: return 'bolovanje';
    case VacationType.PaidLeave: return 'plaćeni dopust';
    default: return 'nepoznato';
  }
};

export interface Payroll {
  id: string;
  employeeId: string;
  employeeName?: string;
  month: number;
  year: number;
  grossSalary: number;
  netSalary: number;
  tax: number;
  socialContributions: number;
  paid: boolean;
  paymentDate?: string;
  notes?: string;
}

export interface Loan {
    id: string;
    employeeId: string;
    employeeName?: string;
    date: string;
    amount: number;
    description: string;
    status: 'aktivna' | 'otplaćena';
}
