import { z } from 'zod';

export const loginSchema = z.object({
  username: z
    .string()
    .min(3, 'Korisničko ime mora imati najmanje 3 znaka')
    .max(50, 'Korisničko ime ne smije biti duže od 50 znakova')
    .regex(/^[a-zA-Z0-9_]+$/, 'Korisničko ime može sadržavati samo slova, brojeve i podvlaku'),
  password: z
    .string()
    .min(6, 'Lozinka mora imati najmanje 6 znakova')
    .max(100, 'Lozinka ne smije biti duža od 100 znakova'),
});

export const registerSchema = z.object({
  username: z
    .string()
    .min(3, 'Korisničko ime mora imati najmanje 3 znaka')
    .max(50, 'Korisničko ime ne smije biti duže od 50 znakova')
    .regex(/^[a-zA-Z0-9_]+$/, 'Korisničko ime može sadržavati samo slova, brojeve i podvlaku'),
  email: z
    .string()
    .email('Neispravna email adresa')
    .max(100, 'Email ne smije biti dulji od 100 znakova'),
  password: z
    .string()
    .min(6, 'Lozinka mora imati najmanje 6 znakova')
    .max(100, 'Lozinka ne smije biti duža od 100 znakova')
    .regex(/[A-Z]/, 'Lozinka mora sadržavati barem jedno veliko slovo')
    .regex(/[a-z]/, 'Lozinka mora sadržavati barem jedno malo slovo')
    .regex(/[0-9]/, 'Lozinka mora sadržavati barem jedan broj'),
  confirmPassword: z.string(),
}).refine((data) => data.password === data.confirmPassword, {
  message: 'Lozinke se ne podudaraju',
  path: ['confirmPassword'],
});

export type LoginFormData = z.infer<typeof loginSchema>;
export type RegisterFormData = z.infer<typeof registerSchema>;
