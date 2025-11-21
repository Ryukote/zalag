import { z } from 'zod';

export const clientSchema = z.object({
  name: z
    .string()
    .min(1, 'Ime klijenta je obavezno')
    .max(200, 'Ime ne smije biti duže od 200 znakova'),
  oib: z
    .string()
    .length(11, 'OIB mora imati točno 11 znakova')
    .regex(/^\d{11}$/, 'OIB mora sadržavati samo brojeve'),
  address: z
    .string()
    .max(300, 'Adresa ne smije biti duža od 300 znakova')
    .optional(),
  city: z
    .string()
    .max(100, 'Grad ne smije biti dulji od 100 znakova')
    .optional(),
  postalCode: z
    .string()
    .max(10, 'Poštanski broj ne smije biti dulji od 10 znakova')
    .optional(),
  phone: z
    .string()
    .max(20, 'Telefon ne smije biti dulji od 20 znakova')
    .optional(),
  email: z
    .string()
    .email('Neispravan format email adrese')
    .max(100, 'Email ne smije biti dulji od 100 znakova')
    .optional()
    .or(z.literal('')),
  idCardNumber: z
    .string()
    .max(20, 'Broj osobne ne smije biti dulji od 20 znakova')
    .optional(),
});

export const pledgeSchema = z.object({
  clientId: z.string().min(1, 'Klijent je obavezan'),
  itemName: z
    .string()
    .min(1, 'Naziv predmeta je obavezan')
    .max(200, 'Naziv ne smije biti dulji od 200 znakova'),
  itemDescription: z
    .string()
    .max(1000, 'Opis ne smije biti dulji od 1000 znakova')
    .optional(),
  estimatedValue: z
    .number()
    .positive('Procjena vrijednosti mora biti veća od 0'),
  loanAmount: z
    .number()
    .positive('Iznos zajma mora biti veći od 0'),
  interestRate: z
    .number()
    .min(0, 'Kamatna stopa ne može biti negativna')
    .max(100, 'Kamatna stopa ne može biti veća od 100%'),
  durationDays: z
    .number()
    .int('Trajanje mora biti cijeli broj')
    .min(1, 'Trajanje mora biti barem 1 dan')
    .max(365, 'Trajanje ne može biti dulje od 365 dana'),
  weight: z
    .number()
    .positive('Težina mora biti veća od 0')
    .optional(),
  fineness: z
    .number()
    .positive('Finoća mora biti veća od 0')
    .max(999, 'Finoća ne može biti veća od 999')
    .optional(),
}).refine((data) => data.loanAmount <= data.estimatedValue, {
  message: 'Iznos zajma ne može biti veći od procijenjene vrijednosti',
  path: ['loanAmount'],
});

export const articleSchema = z.object({
  name: z
    .string()
    .min(1, 'Naziv artikla je obavezan')
    .max(200, 'Naziv ne smije biti dulji od 200 znakova'),
  code: z
    .string()
    .max(50, 'Šifra ne smije biti dulja od 50 znakova')
    .optional(),
  barcode: z
    .string()
    .max(50, 'Barkod ne smije biti dulji od 50 znakova')
    .optional(),
  purchasePrice: z
    .number()
    .min(0, 'Nabavna cijena ne može biti negativna'),
  sellingPrice: z
    .number()
    .min(0, 'Prodajna cijena ne može biti negativna'),
  minimumStock: z
    .number()
    .int('Minimalna zaliha mora biti cijeli broj')
    .min(0, 'Minimalna zaliha ne može biti negativna'),
  currentStock: z
    .number()
    .int('Trenutna zaliha mora biti cijeli broj')
    .min(0, 'Trenutna zaliha ne može biti negativna'),
  weight: z
    .number()
    .positive('Težina mora biti veća od 0')
    .optional(),
  fineness: z
    .number()
    .positive('Finoća mora biti veća od 0')
    .max(999, 'Finoća ne može biti veća od 999')
    .optional(),
});

export const saleSchema = z.object({
  clientId: z.string().min(1, 'Klijent je obavezan'),
  articleId: z.string().min(1, 'Artikl je obavezan'),
  quantity: z
    .number()
    .int('Količina mora biti cijeli broj')
    .positive('Količina mora biti veća od 0'),
  unitPrice: z
    .number()
    .positive('Jedinična cijena mora biti veća od 0'),
  paymentMethod: z
    .string()
    .min(1, 'Način plaćanja je obavezan')
    .max(50, 'Način plaćanja ne smije biti dulji od 50 znakova'),
  invoiceNumber: z
    .string()
    .max(50, 'Broj računa ne smije biti dulji od 50 znakova')
    .optional(),
  notes: z
    .string()
    .max(500, 'Napomena ne smije biti dulja od 500 znakova')
    .optional(),
});

export type ClientFormData = z.infer<typeof clientSchema>;
export type PledgeFormData = z.infer<typeof pledgeSchema>;
export type ArticleFormData = z.infer<typeof articleSchema>;
export type SaleFormData = z.infer<typeof saleSchema>;
