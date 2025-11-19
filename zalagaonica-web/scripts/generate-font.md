# How to Add Roboto Font for Croatian Characters

Since the standard PDF fonts don't support Croatian characters, you need to add Roboto font. Follow these simple steps:

## Step 1: Convert Roboto Font to Base64

1. Visit: https://rawgit.com/MrRio/jsPDF/master/fontconverter/fontconverter.html
2. Upload Roboto-Regular.ttf (download from https://fonts.google.com/specimen/Roboto)
3. Click "Create" button
4. The converter will generate a JavaScript file with base64 encoded font

## Step 2: Copy the Generated Code

The converter will generate something like:

```javascript
(function (jsPDFAPI) {
    "use strict";
    jsPDFAPI.addFileToVFS('Roboto-Regular.ttf','AAAEAAAA... [very long base64 string]');
    jsPDFAPI.addFont('Roboto-Regular.ttf', 'Roboto', 'normal');
})(jsPDF.API);
```

## Step 3: Extract the Base64 String

Copy ONLY the base64 string (the part between the quotes after 'Roboto-Regular.ttf',)

## Step 4: Add to Your Project

1. Open: `src/utils/fonts/roboto-base64.ts`
2. Replace the empty string with your base64:
   ```typescript
   export const robotoRegularBase64 = 'YOUR_VERY_LONG_BASE64_STRING_HERE';
   ```

That's it! The font will now work automatically.

## Quick Test

After adding the font, restart the app and generate a pledge PDF. Croatian characters (č, ć, š, ž, đ) should display correctly.
