// Roboto Regular font in base64 format for jsPDF
// The font supports Croatian characters (č, ć, š, ž, đ)
//
// TO ADD FULL ROBOTO FONT:
// 1. Download Roboto-Regular.ttf from Google Fonts
// 2. Convert to base64: https://www.giftofspeed.com/base64-encoder/
// 3. Replace the placeholder below with the full base64 string
//
// For now, we'll use a fallback approach with standard PDF fonts

export const robotoRegularBase64: string = '';

// Function to add Roboto font to jsPDF
export const addRobotoFont = (doc: any): void => {
  // If you have the base64 string, uncomment and use this:
  // if (robotoRegularBase64) {
  //   doc.addFileToVFS('Roboto-Regular.ttf', robotoRegularBase64);
  //   doc.addFont('Roboto-Regular.ttf', 'Roboto', 'normal');
  //   doc.setFont('Roboto');
  // }

  // Fallback: Use courier which has slightly better UTF-8 support than helvetica/times
  // Note: For full Croatian character support, you need to add the Roboto font base64 above
  doc.setFont('courier', 'normal');
};

export default addRobotoFont;
