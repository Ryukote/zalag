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
  buyerBox: {
    width: '45%',
  },
  buyerTitle: {
    fontSize: 11,
    fontWeight: 'bold',
    marginBottom: 5,
  },
  buyerText: {
    fontSize: 9,
    marginBottom: 2,
  },
  metadata: {
    fontSize: 9,
    marginBottom: 15,
  },
  title: {
    fontSize: 16,
    fontWeight: 'bold',
    textAlign: 'center',
    marginBottom: 20,
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
    fontSize: 9,
  },
  tableRow: {
    flexDirection: 'row',
    borderBottom: '1px solid black',
    borderLeft: '1px solid black',
    borderRight: '1px solid black',
    fontSize: 9,
  },
  col1: { width: '5%', padding: 4, borderRight: '1px solid black', textAlign: 'center' },
  col2: { width: '30%', padding: 4, borderRight: '1px solid black' },
  col3: { width: '18%', padding: 4, borderRight: '1px solid black' },
  col4: { width: '12%', padding: 4, borderRight: '1px solid black', textAlign: 'center' },
  col5: { width: '10%', padding: 4, borderRight: '1px solid black', textAlign: 'center' },
  col6: { width: '12.5%', padding: 4, borderRight: '1px solid black', textAlign: 'right' },
  col7: { width: '12.5%', padding: 4, textAlign: 'right' },
  total: {
    fontSize: 12,
    fontWeight: 'bold',
    textAlign: 'right',
    marginBottom: 5,
  },
  exchangeInfo: {
    fontSize: 9,
    textAlign: 'right',
    marginBottom: 2,
  },
  statementTitle: {
    fontSize: 9,
    fontWeight: 'bold',
    marginBottom: 5,
    marginTop: 10,
  },
  statementText: {
    fontSize: 8,
    marginBottom: 10,
    textAlign: 'justify',
    lineHeight: 1.4,
  },
  signatures: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    marginTop: 15,
  },
  signatureBlock: {
    width: '45%',
  },
  signatureLabel: {
    fontSize: 9,
    marginBottom: 5,
  },
  signatureLine: {
    borderBottom: '1px solid black',
    width: '100%',
    marginTop: 40,
  },
  signatureStamp: {
    fontSize: 8,
    marginTop: 3,
  },
  deposit: {
    fontSize: 10,
    fontWeight: 'bold',
    marginTop: 15,
  },
  pageNumber: {
    position: 'absolute',
    fontSize: 8,
    bottom: 20,
    right: 40,
  },
});

export interface ReservationReceiptDataReact {
  documentNumber: string;
  documentDate: Date;
  buyer: {
    name: string;
    address?: string;
    city?: string;
    oib?: string;
  };
  warehouse: string;
  items: Array<{
    name: string;
    code: string;
    quantity: number;
    unitOfMeasure: string;
    pricePerUnit: number;
    mpc: number;
  }>;
  totalAmount: number;
  exchangeRate: number;
  totalInKuna: number;
  reservationDeposit: number;
  employeeName?: string;
}

const formatCurrency = (amount: number, currency: string = '€'): string => {
  return `${amount.toFixed(2).replace('.', ',')} ${currency}`;
};

const formatDate = (date: Date): string => {
  const day = String(date.getDate()).padStart(2, '0');
  const month = String(date.getMonth() + 1).padStart(2, '0');
  const year = date.getFullYear();
  return `${day}.${month}.${year}`;
};

const ReservationReceiptDocument: React.FC<{ data: ReservationReceiptDataReact }> = ({ data }) => {
  return (
    <Document>
      <Page size="A4" style={styles.page}>
        {/* Header with company and buyer info */}
        <View style={styles.header}>
          <View style={styles.companyBox}>
            <Text style={styles.companyTitle}>KUPAC:</Text>
            <Text style={styles.companyText}>PAWN SHOPS d.o.o.</Text>
            <Text style={styles.companyText}>P.J. Horvačanska cesta 25, Zagreb</Text>
            <Text style={styles.companyText}>Logorište 11a</Text>
            <Text style={styles.companyText}>47000 Karlovac</Text>
            <Text style={styles.companyText}>OIB: 51659874442</Text>
            <Text style={styles.companyText}>Tel: 092 500 8000</Text>
          </View>

          <View style={styles.buyerBox}>
            <Text style={styles.buyerTitle}>Prodavatelj:</Text>
            <Text style={styles.buyerText}>{data.buyer.name}</Text>
            {data.buyer.address && <Text style={styles.buyerText}>{data.buyer.address}</Text>}
            {data.buyer.city && <Text style={styles.buyerText}>{data.buyer.city}</Text>}
            {data.buyer.oib && <Text style={styles.buyerText}>OIB: {data.buyer.oib}</Text>}
          </View>
        </View>

        {/* Metadata */}
        <View style={styles.metadata}>
          <Text>Datum dokumenta: {formatDate(data.documentDate)}</Text>
          <Text>Skladište: {data.warehouse}</Text>
        </View>

        {/* Title */}
        <Text style={styles.title}>
          OTKUPNI BLOK RAB. DOBRA S REZERVACIJOM: {data.documentNumber}
        </Text>

        {/* Items Table */}
        <View style={styles.table}>
          {/* Table Header */}
          <View style={styles.tableHeader}>
            <Text style={styles.col1}></Text>
            <Text style={styles.col2}>naziv artikla</Text>
            <Text style={styles.col3}>oznaka / šifra</Text>
            <Text style={styles.col4}>jed. mjere</Text>
            <Text style={styles.col5}>količina</Text>
            <Text style={styles.col6}>MPC</Text>
            <Text style={styles.col7}>MPV (eur)</Text>
          </View>

          {/* Table Rows */}
          {data.items.map((item, index) => (
            <View key={index} style={styles.tableRow}>
              <Text style={styles.col1}>{index + 1}</Text>
              <Text style={styles.col2}>{item.name}</Text>
              <Text style={styles.col3}>{item.code}</Text>
              <Text style={styles.col4}>{item.unitOfMeasure}</Text>
              <Text style={styles.col5}>{item.quantity}</Text>
              <Text style={styles.col6}>{formatCurrency(item.pricePerUnit)}</Text>
              <Text style={styles.col7}>{formatCurrency(item.mpc)}</Text>
            </View>
          ))}
        </View>

        {/* Total */}
        <Text style={styles.total}>Ukupno: {formatCurrency(data.totalAmount)}</Text>

        {/* Exchange rate info */}
        <Text style={styles.exchangeInfo}>
          prema tečaju: 1 euro = {data.exchangeRate.toFixed(5)} kuna
        </Text>
        <Text style={styles.exchangeInfo}>
          Ukupno: {formatCurrency(data.totalInKuna, 'kn')}
        </Text>

        {/* Legal statement */}
        <Text style={styles.statementTitle}>Izjava prodavatelja:</Text>
        <Text style={styles.statementText}>
          Izjavljujem da je predmet isključivo moje vlasništvo.
          Suglasan sam da: PAWN SHOPS d.o.o. ne odgovora za dokumente, podatke, informacije, autorska i vlasnička
          prava trećih osoba te bilo koji drugi sadržaj, kao ni mogućih štetu prema prodavatelju ili bilo kojoj
          trećoj strani koja bi mogla nastati zbog upotrebe istih.
        </Text>
        <Text style={styles.statementText}>
          Izjavljujem da nisam porezni obveznik po čl. 6. Zakona o porezu na dodanu vrijednost, niti sam obveznik
          fiskalizacije.
        </Text>

        <Text style={styles.statementTitle}>REZERVACIJA ROBE: PRAVO OTKUPA ROBE DO DATUMA ISTEKA REZERVACIJE O</Text>
        <Text style={styles.statementText}>
          Kapara za pravo rezervacije i pravo otkupa nije plaćena.
        </Text>
        <Text style={styles.statementText}>
          PRODAVATELJ i KUPAC sporazumno utvrđuju da će PRODAVATELJ naslaviti OTKUP po ovoj rezervaciji 24 sata
          ranije.
          Kupac se obavezuje da PLU-otplati OTKUP od OTPLATE bez dužih prava proizvoda s čijelosti do isteka
          rezervacije, KUPAC zadržava robu, te obje strane nemaju međusobnih potražlvanja vezano uz ovu kupovinu.
          U slučaju ispunjenja svih uvjeta otplate i poštivanje rezervacije marže po članku 9S.a Zakona o porezu
          na dodanu vrijednost.
          Roba je kapirana po principu viđeno-kupljeno te su KUPAC i PRODAVATELJ sami dužni utvrdili ispravnost
          proizvoda prije kupnje.
        </Text>
        <Text style={styles.statementText}>
          PRODAVATELJ i KUPAC sporazumno utvrđuju da će PRODAVATELJ naslaviti OTKUP po ovoj rezervaciji 24 sata
          ranije.
        </Text>

        {/* Signatures */}
        <View style={styles.signatures}>
          <View style={styles.signatureBlock}>
            <Text style={styles.signatureLabel}>Izradio: {data.employeeName || 'Tin Matija'}</Text>
            <View style={styles.signatureLine} />
            <Text style={styles.signatureStamp}>m.p.</Text>
          </View>

          <View style={styles.signatureBlock}>
            <Text style={styles.signatureLabel}>Prodavatelj:</Text>
            <Text style={styles.signatureLabel}>{data.buyer.name}</Text>
            <View style={styles.signatureLine} />
          </View>
        </View>

        {/* Reservation deposit */}
        <Text style={styles.deposit}>
          Kapara pri otkupu: {formatCurrency(data.reservationDeposit)}
        </Text>

        {/* Page number */}
        <Text style={styles.pageNumber}>1 od 1</Text>
      </Page>
    </Document>
  );
};

export const generateReservationReceiptReact = async (data: ReservationReceiptDataReact): Promise<void> => {
  const blob = await pdf(<ReservationReceiptDocument data={data} />).toBlob();
  const url = URL.createObjectURL(blob);
  const link = document.createElement('a');
  link.href = url;
  link.download = `otkupni-blok-rezervacija-${data.documentNumber}.pdf`;
  link.click();
  URL.revokeObjectURL(url);
};
