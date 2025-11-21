-- Migration: Add missing fields to Sale entity
-- Date: 2024-11-21
-- Description: Add Quantity, UnitPrice, TotalAmount, PaymentMethod, InvoiceNumber, Notes, SaleDate, and UserId to Sales table

-- Add new columns to Sales table
ALTER TABLE "Sales" ADD COLUMN "SaleDate" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP;
ALTER TABLE "Sales" ADD COLUMN "Quantity" INTEGER NOT NULL DEFAULT 1;
ALTER TABLE "Sales" ADD COLUMN "UnitPrice" NUMERIC NOT NULL DEFAULT 0;
ALTER TABLE "Sales" ADD COLUMN "TotalAmount" NUMERIC NOT NULL DEFAULT 0;
ALTER TABLE "Sales" ADD COLUMN "PaymentMethod" VARCHAR(50);
ALTER TABLE "Sales" ADD COLUMN "InvoiceNumber" VARCHAR(50);
ALTER TABLE "Sales" ADD COLUMN "Notes" VARCHAR(500);
ALTER TABLE "Sales" ADD COLUMN "UserId" UUID;

-- Migrate existing data: Copy SalePrice to UnitPrice and TotalAmount
UPDATE "Sales" SET
    "UnitPrice" = "SalePrice",
    "TotalAmount" = "SalePrice" * "Quantity",
    "SaleDate" = "CreatedAt"
WHERE "UnitPrice" = 0 OR "TotalAmount" = 0;

-- Optional: Add index for faster queries on SaleDate
CREATE INDEX "IX_Sales_SaleDate" ON "Sales" ("SaleDate");

-- Optional: Add index for UserId foreign key
CREATE INDEX "IX_Sales_UserId" ON "Sales" ("UserId");
