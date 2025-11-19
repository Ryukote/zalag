export type VehicleEventType = 'Servis' | 'Registracija' | 'Osiguranje' | 'Kazna';

export interface Vehicle {
  id: string;
  make: string; // Marka
  model: string;
  year: number;
  registrationPlate: string;
}

export interface VehicleEvent {
  id: string;
  vehicleId: string;
  date: string;
  type: VehicleEventType;
  description: string;
  cost: number;
  expiryDate?: string; // Za registraciju i osiguranje
  status?: 'plaćeno' | 'neplaćeno'; // Za kazne
}
