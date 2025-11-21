import { clientSchema, pledgeSchema, articleSchema, saleSchema } from './clientSchemas';

describe('clientSchema', () => {
  it('should validate a valid client', () => {
    const validClient = {
      name: 'Ivan Horvat',
      oib: '12345678901',
      address: 'Ilica 1',
      city: 'Zagreb',
      postalCode: '10000',
      phone: '+385911234567',
      email: 'ivan@example.com',
      idCardNumber: '123456789',
    };

    const result = clientSchema.safeParse(validClient);
    expect(result.success).toBe(true);
  });

  it('should reject client with invalid OIB length', () => {
    const invalidClient = {
      name: 'Ivan Horvat',
      oib: '123456', // Too short
      address: 'Ilica 1',
    };

    const result = clientSchema.safeParse(invalidClient);
    expect(result.success).toBe(false);
    if (!result.success) {
      expect(result.error.issues[0].message).toContain('11 znakova');
    }
  });

  it('should reject client with non-numeric OIB', () => {
    const invalidClient = {
      name: 'Ivan Horvat',
      oib: '1234567890A', // Contains letter
    };

    const result = clientSchema.safeParse(invalidClient);
    expect(result.success).toBe(false);
    if (!result.success) {
      expect(result.error.issues[0].message).toContain('brojeve');
    }
  });

  it('should reject client without name', () => {
    const invalidClient = {
      name: '',
      oib: '12345678901',
    };

    const result = clientSchema.safeParse(invalidClient);
    expect(result.success).toBe(false);
  });

  it('should accept client with valid email', () => {
    const validClient = {
      name: 'Ivan Horvat',
      oib: '12345678901',
      email: 'ivan@example.com',
    };

    const result = clientSchema.safeParse(validClient);
    expect(result.success).toBe(true);
  });

  it('should reject client with invalid email', () => {
    const invalidClient = {
      name: 'Ivan Horvat',
      oib: '12345678901',
      email: 'invalid-email',
    };

    const result = clientSchema.safeParse(invalidClient);
    expect(result.success).toBe(false);
  });
});

describe('pledgeSchema', () => {
  it('should validate a valid pledge', () => {
    const validPledge = {
      clientId: '123e4567-e89b-12d3-a456-426614174000',
      itemName: 'Gold Ring',
      itemDescription: '18K Gold Ring',
      estimatedValue: 5000,
      loanAmount: 3000,
      interestRate: 5,
      durationDays: 30,
      weight: 10.5,
      fineness: 750,
    };

    const result = pledgeSchema.safeParse(validPledge);
    expect(result.success).toBe(true);
  });

  it('should reject pledge where loanAmount exceeds estimatedValue', () => {
    const invalidPledge = {
      clientId: '123e4567-e89b-12d3-a456-426614174000',
      itemName: 'Gold Ring',
      estimatedValue: 3000,
      loanAmount: 5000, // Greater than estimatedValue
      interestRate: 5,
      durationDays: 30,
    };

    const result = pledgeSchema.safeParse(invalidPledge);
    expect(result.success).toBe(false);
    if (!result.success) {
      expect(result.error.issues[0].message).toContain('procijenjene vrijednosti');
    }
  });

  it('should reject pledge with negative estimatedValue', () => {
    const invalidPledge = {
      clientId: '123e4567-e89b-12d3-a456-426614174000',
      itemName: 'Gold Ring',
      estimatedValue: -1000,
      loanAmount: 500,
      interestRate: 5,
      durationDays: 30,
    };

    const result = pledgeSchema.safeParse(invalidPledge);
    expect(result.success).toBe(false);
  });

  it('should reject pledge with duration over 365 days', () => {
    const invalidPledge = {
      clientId: '123e4567-e89b-12d3-a456-426614174000',
      itemName: 'Gold Ring',
      estimatedValue: 5000,
      loanAmount: 3000,
      interestRate: 5,
      durationDays: 400, // Too long
    };

    const result = pledgeSchema.safeParse(invalidPledge);
    expect(result.success).toBe(false);
  });

  it('should reject pledge with interestRate over 100', () => {
    const invalidPledge = {
      clientId: '123e4567-e89b-12d3-a456-426614174000',
      itemName: 'Gold Ring',
      estimatedValue: 5000,
      loanAmount: 3000,
      interestRate: 150, // Too high
      durationDays: 30,
    };

    const result = pledgeSchema.safeParse(invalidPledge);
    expect(result.success).toBe(false);
  });
});

describe('articleSchema', () => {
  it('should validate a valid article', () => {
    const validArticle = {
      name: 'Gold Bracelet',
      code: 'ART001',
      barcode: '1234567890123',
      purchasePrice: 1000,
      sellingPrice: 1500,
      minimumStock: 5,
      currentStock: 10,
      weight: 25.5,
      fineness: 750,
    };

    const result = articleSchema.safeParse(validArticle);
    expect(result.success).toBe(true);
  });

  it('should reject article with negative prices', () => {
    const invalidArticle = {
      name: 'Gold Bracelet',
      purchasePrice: -100,
      sellingPrice: 1500,
      minimumStock: 5,
      currentStock: 10,
    };

    const result = articleSchema.safeParse(invalidArticle);
    expect(result.success).toBe(false);
  });

  it('should reject article with negative stock', () => {
    const invalidArticle = {
      name: 'Gold Bracelet',
      purchasePrice: 1000,
      sellingPrice: 1500,
      minimumStock: 5,
      currentStock: -10,
    };

    const result = articleSchema.safeParse(invalidArticle);
    expect(result.success).toBe(false);
  });

  it('should reject article without name', () => {
    const invalidArticle = {
      name: '',
      purchasePrice: 1000,
      sellingPrice: 1500,
      minimumStock: 5,
      currentStock: 10,
    };

    const result = articleSchema.safeParse(invalidArticle);
    expect(result.success).toBe(false);
  });
});

describe('saleSchema', () => {
  it('should validate a valid sale', () => {
    const validSale = {
      clientId: '123e4567-e89b-12d3-a456-426614174000',
      articleId: '123e4567-e89b-12d3-a456-426614174001',
      quantity: 2,
      unitPrice: 100,
      paymentMethod: 'cash',
      invoiceNumber: 'INV-001',
      notes: 'Test sale',
    };

    const result = saleSchema.safeParse(validSale);
    expect(result.success).toBe(true);
  });

  it('should reject sale with zero quantity', () => {
    const invalidSale = {
      clientId: '123e4567-e89b-12d3-a456-426614174000',
      articleId: '123e4567-e89b-12d3-a456-426614174001',
      quantity: 0,
      unitPrice: 100,
      paymentMethod: 'cash',
    };

    const result = saleSchema.safeParse(invalidSale);
    expect(result.success).toBe(false);
  });

  it('should reject sale with negative unitPrice', () => {
    const invalidSale = {
      clientId: '123e4567-e89b-12d3-a456-426614174000',
      articleId: '123e4567-e89b-12d3-a456-426614174001',
      quantity: 2,
      unitPrice: -100,
      paymentMethod: 'cash',
    };

    const result = saleSchema.safeParse(invalidSale);
    expect(result.success).toBe(false);
  });

  it('should reject sale without paymentMethod', () => {
    const invalidSale = {
      clientId: '123e4567-e89b-12d3-a456-426614174000',
      articleId: '123e4567-e89b-12d3-a456-426614174001',
      quantity: 2,
      unitPrice: 100,
      paymentMethod: '',
    };

    const result = saleSchema.safeParse(invalidSale);
    expect(result.success).toBe(false);
  });
});
