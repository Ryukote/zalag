# Croatian Character Support in PDFs

## Current Solution

The PDF generator now uses **Courier font** which has better support for Croatian special characters (č, ć, š, ž, đ) compared to Helvetica or Times.

## Testing

Generate a PDF from the Pledge List page (Knjiga Zaloga) to test Croatian character rendering. Click the document icon next to any pledge to generate the PDF.

## If Courier Font Doesn't Work Properly

If Croatian characters still don't display correctly, you can add Roboto font which has full UTF-8 support:

### Step 1: Download Roboto Font

1. Go to [Google Fonts](https://fonts.google.com/specimen/Roboto)
2. Download Roboto Regular (Roboto-Regular.ttf)
3. Or download directly from [GitHub](https://github.com/google/roboto/blob/main/src/hinted/Roboto-Regular.ttf)

### Step 2: Convert to Base64

1. Go to https://www.giftofspeed.com/base64-encoder/
2. Upload your Roboto-Regular.ttf file
3. Click "Encode file to Base64"
4. Copy the base64 string

### Step 3: Add to Project

1. Open `src/utils/fonts/roboto-base64.ts`
2. Replace the empty string in `robotoRegularBase64 = ''` with your base64 string:
   ```typescript
   export const robotoRegularBase64 = 'YOUR_BASE64_STRING_HERE';
   ```

### Step 4: Enable Roboto Font

1. Open `src/utils/pdfGenerator.ts`
2. Find the `createPDF` function
3. Replace the courier font line with:
   ```typescript
   import { addRobotoFont } from './fonts/roboto-base64';

   export const createPDF = (orientation: 'portrait' | 'landscape' = 'portrait'): jsPDF => {
     const doc = new jsPDF({
       orientation,
       unit: 'mm',
       format: 'a4',
       putOnlyUsedFonts: true,
       compress: true
     });

     addRobotoFont(doc); // This will add and set Roboto font

     return doc;
   };
   ```

4. In `roboto-base64.ts`, uncomment the code in the `addRobotoFont` function:
   ```typescript
   export const addRobotoFont = (doc: any): void => {
     if (robotoRegularBase64) {
       doc.addFileToVFS('Roboto-Regular.ttf', robotoRegularBase64);
       doc.addFont('Roboto-Regular.ttf', 'Roboto', 'normal');
       doc.setFont('Roboto');
     }
   };
   ```

5. Update all font references in the codebase from 'courier' to 'Roboto'

## Alternative Solution: Font Subsetting

If the base64 string is too large (>100KB), you can create a font subset containing only the characters you need:

1. Use [Font Squirrel Webfont Generator](https://www.fontsquirrel.com/tools/webfont-generator)
2. Upload Roboto-Regular.ttf
3. Select "Expert" mode
4. Under "Subsetting", choose "Custom Subsetting"
5. Include: Latin Extended A and Latin Extended B character sets
6. Download and convert the subset to base64

This will significantly reduce the file size while maintaining Croatian character support.

## Supported Characters

Croatian alphabet includes these special characters:
- Č (č) - C with caron
- Ć (ć) - C with acute
- Š (š) - S with caron
- Ž (ž) - Z with caron
- Đ (đ) - D with stroke

All of these should render correctly with either Courier (current) or Roboto (if added).
