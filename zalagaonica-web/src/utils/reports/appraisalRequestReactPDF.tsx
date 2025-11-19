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
  clientBox: {
    width: '45%',
  },
  clientTitle: {
    fontSize: 11,
    fontWeight: 'bold',
    marginBottom: 5,
  },
  clientText: {
    fontSize: 9,
    marginBottom: 2,
  },
  metadata: {
    fontSize: 9,
    marginBottom: 15,
    textAlign: 'right',
  },
  metadataLine: {
    marginBottom: 2,
  },
  title: {
    fontSize: 16,
    fontWeight: 'bold',
    textAlign: 'center',
    marginBottom: 20,
  },
  introText: {
    fontSize: 9,
    marginBottom: 15,
    textAlign: 'justify',
    lineHeight: 1.4,
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
  col1: { width: '10%', padding: 4, borderRight: '1px solid black', textAlign: 'center' },
  col2: { width: '40%', padding: 4, borderRight: '1px solid black' },
  col3: { width: '25%', padding: 4, borderRight: '1px solid black' },
  col4: { width: '12.5%', padding: 4, borderRight: '1px solid black', textAlign: 'center' },
  col5: { width: '12.5%', padding: 4, textAlign: 'center' },
  employeeSection: {
    marginBottom: 15,
  },
  employeeText: {
    fontSize: 9,
    marginBottom: 2,
  },
  clientLabel: {
    fontSize: 9,
    textAlign: 'right',
    marginBottom: 15,
  },
  statementTitle: {
    fontSize: 9,
    fontWeight: 'bold',
    marginBottom: 5,
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
    marginTop: 40,
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
  pageNumber: {
    position: 'absolute',
    fontSize: 8,
    bottom: 20,
    right: 40,
  },
});

export interface AppraisalRequestDataReact {
  documentNumber: string;
  documentDate: Date;
  client: {
    name: string;
    address?: string;
    city?: string;
    oib?: string;
  };
  items: Array<{
    name: string;
    description: string;
    code: string;
    quantity: number;
    unitOfMeasure: string;
  }>;
  warehouse: string;
  employeeName?: string;
}

const formatDate = (date: Date): string => {
  const day = String(date.getDate()).padStart(2, '0');
  const month = String(date.getMonth() + 1).padStart(2, '0');
  const year = date.getFullYear();
  return `${day}.${month}.${year}`;
};

const AppraisalRequestDocument: React.FC<{ data: AppraisalRequestDataReact }> = ({ data }) => {
  return (
    <Document>
      <Page size="A4" style={styles.page}>
        {/* Header with company and client info */}
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

          <View style={styles.clientBox}>
            <Text style={styles.clientTitle}>Prodavatelj:</Text>
            <Text style={styles.clientText}>{data.client.name}</Text>
            {data.client.address && <Text style={styles.clientText}>{data.client.address}</Text>}
            {data.client.city && <Text style={styles.clientText}>{data.client.city}</Text>}
            {data.client.oib && <Text style={styles.clientText}>OIB: {data.client.oib}</Text>}
          </View>
        </View>

        {/* Metadata */}
        <View style={styles.metadata}>
          <Text style={styles.metadataLine}>Datum ul. dokumenta: {formatDate(data.documentDate)}</Text>
          <Text style={styles.metadataLine}>Datum isteka:</Text>
          <Text style={styles.metadataLine}>Skladište: {data.warehouse}</Text>
        </View>

        {/* Title */}
        <Text style={styles.title}>
          ZAHTJEV ZA PROCJENOM PREDMETA: {data.documentNumber}
        </Text>

        {/* Introduction text */}
        <Text style={styles.introText}>
          Molim tvrtku PAWN SHOPS d.o.o. da u svrhu mogućeg otkupa predmeta u mom vlasništvu, izvrši potrebna
          ispitivanja (vizualna, termička, kemijska ili ...), kako bi utvrdila kakvoću predmeta koje nudim.
          Prihvaćam činjenicom da tvrtka PAWN SHOPS d.o.o. nije dužna plaćati ni kakvoću predmeta po izvršenim
          provjerama ili procjenama ili procjeni izvršiti otkup predmeta koje nudim i na kojima je radila procjena.
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
          </View>

          {/* Table Rows */}
          {data.items.map((item, index) => (
            <View key={index} style={styles.tableRow}>
              <Text style={styles.col1}>{index + 1}</Text>
              <Text style={styles.col2}>{item.name}</Text>
              <Text style={styles.col3}>{item.code}</Text>
              <Text style={styles.col4}>{item.unitOfMeasure}</Text>
              <Text style={styles.col5}>{item.quantity}</Text>
            </View>
          ))}
        </View>

        {/* Employee section */}
        <View style={styles.employeeSection}>
          <Text style={styles.employeeText}>IZRADIO: {data.employeeName || 'Domagoj'}</Text>
          <Text style={styles.employeeText}>m.p.</Text>
        </View>

        {/* Client label */}
        <Text style={styles.clientLabel}>KOMITENT</Text>

        {/* Legal statements */}
        <Text style={styles.statementTitle}>Izjava vlasništva:</Text>
        <Text style={styles.statementText}>
          Izjavljujem da je predmet koji prodajem isključivo moje vlasništvo. Izjavljujem da nisam porezni
          obveznik po čl. 6. Zakona o porezu na dodanu vrijednost, niti sam obveznik fiskalizacije.
        </Text>

        <Text style={styles.statementTitle}>Uvjeti procjene:</Text>
        <Text style={styles.statementText}>
          Kupac je dužan robu za procjene podići u roku od 30 dana. Ukoliko se roba sa procjene ne podigne u
          roku od 30 dana, tvrtka PAWN SHOPS d.o.o. zadržava robu, te obje strane nemaju međusobnih potraživanja,
          vezano uz ovu transakciju.
        </Text>

        {/* Page number */}
        <Text style={styles.pageNumber}>1 od 1</Text>
      </Page>
    </Document>
  );
};

export const generateAppraisalRequestReact = async (data: AppraisalRequestDataReact): Promise<void> => {
  const blob = await pdf(<AppraisalRequestDocument data={data} />).toBlob();
  const url = URL.createObjectURL(blob);
  const link = document.createElement('a');
  link.href = url;
  link.download = `zahtjev-procjena-${data.documentNumber}.pdf`;
  link.click();
  URL.revokeObjectURL(url);
};
