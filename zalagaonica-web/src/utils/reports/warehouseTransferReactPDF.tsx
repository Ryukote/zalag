import React from 'react';
import { Document, Page, Text, View, StyleSheet, Font, pdf } from '@react-pdf/renderer';

// Register font with Croatian character support
// Using DejaVu Sans from a reliable CDN
Font.register({
  family: 'DejaVu Sans',
  fonts: [
    {
      src: 'https://kendo.cdn.telerik.com/2017.2.621/styles/fonts/DejaVu/DejaVuSans.ttf',
      fontWeight: 'normal',
    },
    {
      src: 'https://kendo.cdn.telerik.com/2017.2.621/styles/fonts/DejaVu/DejaVuSans-Bold.ttf',
      fontWeight: 'bold',
    },
  ],
});

const styles = StyleSheet.create({
  page: {
    padding: 40,
    fontSize: 10,
    fontFamily: 'DejaVu Sans',
  },
  warehouseInfo: {
    fontSize: 8,
    marginBottom: 15,
  },
  header: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    marginBottom: 20,
  },
  companyBox: {
    border: '1px solid black',
    padding: 8,
    width: '45%',
  },
  companyTitle: {
    fontSize: 11,
    fontWeight: 'bold',
    marginBottom: 5,
  },
  companyText: {
    fontSize: 9,
    marginBottom: 2,
  },
  rightInfo: {
    width: '45%',
    fontSize: 8,
  },
  rightInfoLine: {
    marginBottom: 3,
    textAlign: 'right',
  },
  title: {
    fontSize: 16,
    fontWeight: 'bold',
    textAlign: 'center',
    marginBottom: 15,
  },
  table: {
    width: '100%',
    marginBottom: 15,
  },
  tableHeader: {
    flexDirection: 'row',
    borderBottom: '1px solid black',
    borderTop: '1px solid black',
    borderLeft: '1px solid black',
    borderRight: '1px solid black',
    backgroundColor: '#f0f0f0',
    fontWeight: 'bold',
    fontSize: 7,
  },
  tableRow: {
    flexDirection: 'row',
    borderBottom: '1px solid black',
    borderLeft: '1px solid black',
    borderRight: '1px solid black',
    fontSize: 7,
  },
  tableRowTotal: {
    flexDirection: 'row',
    borderBottom: '1px solid black',
    borderLeft: '1px solid black',
    borderRight: '1px solid black',
    fontSize: 7,
    fontWeight: 'bold',
    backgroundColor: '#f9f9f9',
  },
  col1: { width: '3%', padding: 2, borderRight: '1px solid black', textAlign: 'center' },
  col2: { width: '22%', padding: 2, borderRight: '1px solid black' },
  col3: { width: '6%', padding: 2, borderRight: '1px solid black', textAlign: 'center' },
  col4: { width: '6%', padding: 2, borderRight: '1px solid black', textAlign: 'center' },
  col5: { width: '9%', padding: 2, borderRight: '1px solid black', textAlign: 'right' },
  col6: { width: '9%', padding: 2, borderRight: '1px solid black', textAlign: 'right' },
  col7: { width: '9%', padding: 2, borderRight: '1px solid black', textAlign: 'right' },
  col8: { width: '9%', padding: 2, borderRight: '1px solid black', textAlign: 'right' },
  col9: { width: '8%', padding: 2, borderRight: '1px solid black', textAlign: 'right' },
  col10: { width: '9%', padding: 2, borderRight: '1px solid black', textAlign: 'right' },
  col11: { width: '6%', padding: 2, borderRight: '1px solid black', textAlign: 'right' },
  col12: { width: '9%', padding: 2, textAlign: 'right' },
  note: {
    fontSize: 8,
    fontStyle: 'italic',
    marginBottom: 10,
  },
  bottomSection: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    marginTop: 10,
  },
  leftBottom: {
    width: '35%',
  },
  centerBottom: {
    width: '30%',
  },
  rightBottom: {
    width: '30%',
    alignItems: 'flex-end',
  },
  sectionLabel: {
    fontSize: 8,
    fontWeight: 'bold',
    marginBottom: 5,
  },
  smallText: {
    fontSize: 8,
    marginBottom: 2,
  },
  vatTable: {
    border: '1px solid black',
    marginTop: 5,
  },
  vatTableHeader: {
    flexDirection: 'row',
    borderBottom: '1px solid black',
    backgroundColor: '#f0f0f0',
    fontWeight: 'bold',
    fontSize: 8,
  },
  vatTableRow: {
    flexDirection: 'row',
    fontSize: 8,
  },
  vatCol1: {
    width: '50%',
    padding: 3,
    borderRight: '1px solid black',
    textAlign: 'right',
  },
  vatCol2: {
    width: '50%',
    padding: 3,
    textAlign: 'right',
  },
  vatSummary: {
    fontSize: 8,
    textAlign: 'right',
    marginTop: 5,
  },
  signatureSection: {
    marginTop: 40,
  },
  signatureLabel: {
    fontSize: 8,
  },
  signatureStamp: {
    fontSize: 7,
    marginTop: 2,
  },
  pageNumber: {
    position: 'absolute',
    fontSize: 7,
    bottom: 20,
    right: 40,
    fontStyle: 'italic',
  },
});

export interface WarehouseTransferItemReact {
  name: string;
  description: string;
  code: string;
  unitOfMeasure: string;
  quantity: number;
  invoicePrice: number;
  discountPercent: number;
  discountAmount: number;
  purchasePrice: number;
  marginPercent: number;
  marginAmount: number;
  taxPercent: number;
  retailPrice: number;
}

export interface WarehouseTransferDataReact {
  documentNumber: string;
  documentDate: Date;
  fromWarehouse: string;
  toWarehouse: string;
  client?: {
    name: string;
    oib?: string;
    city?: string;
  };
  items: WarehouseTransferItemReact[];
  totalPurchasePrice: number;
  totalRetailPrice: number;
  note?: string;
  vatInfo?: {
    base: number;
    amount: number;
  };
}

const formatCurrency = (amount: number): string => {
  return `${amount.toFixed(2).replace('.', ',')} €`;
};

const formatDate = (date: Date): string => {
  const day = String(date.getDate()).padStart(2, '0');
  const month = String(date.getMonth() + 1).padStart(2, '0');
  const year = date.getFullYear();
  return `${day}.${month}.${year}`;
};

const WarehouseTransferDocument: React.FC<{ data: WarehouseTransferDataReact }> = ({ data }) => {
  const isNegative = data.totalRetailPrice < 0;
  const totalMargin = data.totalRetailPrice - data.totalPurchasePrice;

  return (
    <Document>
      <Page size="A4" style={styles.page} orientation="landscape">
        {/* Warehouse info */}
        <View style={styles.warehouseInfo}>
          <Text>Skladište: {data.fromWarehouse}</Text>
        </View>

        {/* Header with company and client info */}
        <View style={styles.header}>
          <View style={styles.companyBox}>
            <Text style={styles.companyTitle}>PAWN SHOPS d.o.o.</Text>
            <Text style={styles.companyText}>P.J. Horvačanska cesta 25, Zagreb</Text>
            <Text style={styles.companyText}>Logorište 11a</Text>
            <Text style={styles.companyText}>47000 Karlovac</Text>
            <Text style={styles.companyText}>OIB: 51659874442</Text>
            <Text style={styles.companyText}>Tel: 092 500 8000</Text>
          </View>

          <View style={styles.rightInfo}>
            {data.client && (
              <>
                <Text style={styles.rightInfoLine}>
                  Podaci o dokumentu na osnovu kojeg nastaje ulazni dokument
                </Text>
                <Text style={styles.rightInfoLine}>Komitent: {data.client.name}</Text>
                {data.client.oib && (
                  <Text style={styles.rightInfoLine}>OIB: {data.client.oib}</Text>
                )}
                {data.client.city && (
                  <Text style={styles.rightInfoLine}>{data.client.city}</Text>
                )}
              </>
            )}
            <Text style={styles.rightInfoLine}>
              Ulazni dokument: ob {data.documentNumber.split('-')[0]}
            </Text>
            <Text style={styles.rightInfoLine}>
              Datum ul. dokumenta: {formatDate(data.documentDate)}
            </Text>
            <Text style={styles.rightInfoLine}>
              Datum knjiženja: {formatDate(data.documentDate)}
            </Text>
            <Text style={styles.rightInfoLine}>Datum dospijeca:</Text>
          </View>
        </View>

        {/* Title */}
        <Text style={styles.title}>MEĐUSKLADIŠNJICA: {data.documentNumber}</Text>

        {/* Items Table */}
        <View style={styles.table}>
          {/* Table Header */}
          <View style={styles.tableHeader}>
            <Text style={styles.col1}></Text>
            <Text style={styles.col2}>naziv artikla{'\n'}oznaka / šifra{'\n'}kod artikla</Text>
            <Text style={styles.col3}>jed.{'\n'}mjere</Text>
            <Text style={styles.col4}>količina</Text>
            <Text style={styles.col5}>faktura c{'\n'}faktura vr.</Text>
            <Text style={styles.col6}>rabat (%) rabat vr.</Text>
            <Text style={styles.col7}>nabavna c{'\n'}nabavna vr.</Text>
            <Text style={styles.col8}>marža (%) marža uk.</Text>
            <Text style={styles.col9}>porez %</Text>
            <Text style={styles.col10}>porez iznos</Text>
            <Text style={styles.col11}>mpc (eur)</Text>
            <Text style={styles.col12}>mpv (eur)</Text>
          </View>

          {/* Table Rows */}
          {data.items.map((item, index) => (
            <View key={index} style={styles.tableRow}>
              <Text style={styles.col1}>{index + 1}</Text>
              <Text style={styles.col2}>
                {item.name} ({item.description || ''}) {item.code}
              </Text>
              <Text style={styles.col3}>{item.unitOfMeasure}</Text>
              <Text style={styles.col4}>{item.quantity}</Text>
              <Text style={styles.col5}>{formatCurrency(item.invoicePrice)}</Text>
              <Text style={styles.col6}>
                {item.discountPercent.toFixed(2)} %{'\n'}
                {formatCurrency(item.discountAmount)}
              </Text>
              <Text style={styles.col7}>{formatCurrency(item.purchasePrice)}</Text>
              <Text style={styles.col8}>
                {item.marginPercent.toFixed(2)} %{'\n'}
                {formatCurrency(item.marginAmount)}
              </Text>
              <Text style={styles.col9}>{item.taxPercent.toFixed(0)} %</Text>
              <Text style={styles.col10}></Text>
              <Text style={styles.col11}>{formatCurrency(item.retailPrice / item.quantity)}</Text>
              <Text style={styles.col12}>{formatCurrency(item.retailPrice)}</Text>
            </View>
          ))}

          {/* Totals Row */}
          <View style={styles.tableRowTotal}>
            <Text style={styles.col1}></Text>
            <Text style={styles.col2}></Text>
            <Text style={styles.col3}></Text>
            <Text style={styles.col4}></Text>
            <Text style={styles.col5}>Ukupno (kn):{'\n'}{formatCurrency(Math.abs(data.totalPurchasePrice))}</Text>
            <Text style={styles.col6}></Text>
            <Text style={styles.col7}>
              {formatCurrency(isNegative ? -Math.abs(data.totalPurchasePrice) : Math.abs(data.totalPurchasePrice))}
            </Text>
            <Text style={styles.col8}>{formatCurrency(Math.abs(totalMargin))}</Text>
            <Text style={styles.col9}></Text>
            <Text style={styles.col10}></Text>
            <Text style={styles.col11}></Text>
            <Text style={styles.col12}>{formatCurrency(data.totalRetailPrice)}</Text>
          </View>
        </View>

        {/* Note if provided */}
        {data.note && (
          <View style={styles.note}>
            <Text>Veza na dok.</Text>
          </View>
        )}

        {/* Bottom Section */}
        <View style={styles.bottomSection}>
          <View style={styles.leftBottom}>
            <Text style={styles.sectionLabel}>Izradio: Tin Matija Bertić</Text>
            <Text style={styles.signatureStamp}>m.p.</Text>
          </View>

          {data.vatInfo && (
            <>
              <View style={styles.centerBottom}>
                <Text style={styles.sectionLabel}>porez %</Text>
                <View style={styles.vatTable}>
                  <View style={styles.vatTableHeader}>
                    <Text style={styles.vatCol1}>osnovica</Text>
                    <Text style={styles.vatCol2}>iznos</Text>
                  </View>
                  <View style={styles.vatTableRow}>
                    <Text style={styles.vatCol1}>{formatCurrency(data.vatInfo.base)}</Text>
                    <Text style={styles.vatCol2}>{formatCurrency(data.vatInfo.amount)}</Text>
                  </View>
                </View>
              </View>

              <View style={styles.rightBottom}>
                <Text style={styles.vatSummary}>POREZ na dodanu vrijednost</Text>
                <Text style={styles.vatSummary}>
                  {formatCurrency(data.vatInfo.base)}  {formatCurrency(data.vatInfo.amount)}
                </Text>
                <Text style={styles.vatSummary}>Porezna naknada : 0,00</Text>
              </View>
            </>
          )}
        </View>

        {/* Page number */}
        <Text style={styles.pageNumber}>1 od 1</Text>
      </Page>
    </Document>
  );
};

export const generateWarehouseTransferReact = async (
  data: WarehouseTransferDataReact
): Promise<void> => {
  const blob = await pdf(<WarehouseTransferDocument data={data} />).toBlob();
  const url = URL.createObjectURL(blob);
  const link = document.createElement('a');
  link.href = url;
  link.download = `meduskladisnjica-${data.documentNumber}.pdf`;
  link.click();
  URL.revokeObjectURL(url);
};
