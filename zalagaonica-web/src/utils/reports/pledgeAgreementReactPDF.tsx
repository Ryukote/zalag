import React from 'react';
import { Document, Page, Text, View, StyleSheet, Font, pdf } from '@react-pdf/renderer';

// Register font with Croatian character support
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
    fontSize: 9,
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
    fontSize: 10,
    fontWeight: 'bold',
    marginBottom: 5,
  },
  companyText: {
    fontSize: 8,
    marginBottom: 2,
  },
  clientBox: {
    width: '45%',
  },
  clientTitle: {
    fontSize: 10,
    fontWeight: 'bold',
    marginBottom: 5,
  },
  clientText: {
    fontSize: 8,
    marginBottom: 2,
  },
  metadata: {
    fontSize: 8,
    marginBottom: 15,
    textAlign: 'right',
  },
  title: {
    fontSize: 14,
    fontWeight: 'bold',
    textAlign: 'center',
    marginBottom: 15,
  },
  intro: {
    fontSize: 8,
    marginBottom: 15,
    textAlign: 'justify',
    lineHeight: 1.4,
  },
  sectionTitle: {
    fontSize: 9,
    fontWeight: 'bold',
    marginBottom: 8,
    marginTop: 10,
  },
  detailRow: {
    flexDirection: 'row',
    marginBottom: 5,
    fontSize: 8,
  },
  detailLabel: {
    width: '30%',
    fontWeight: 'bold',
  },
  detailValue: {
    width: '70%',
  },
  termItem: {
    fontSize: 8,
    marginBottom: 5,
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
    fontSize: 8,
    marginBottom: 5,
  },
  signatureLine: {
    borderBottom: '1px solid black',
    width: '100%',
    marginTop: 40,
  },
  signatureStamp: {
    fontSize: 7,
    marginTop: 3,
  },
  pageNumber: {
    position: 'absolute',
    fontSize: 7,
    bottom: 20,
    right: 40,
  },
  footer: {
    position: 'absolute',
    fontSize: 7,
    bottom: 20,
    left: 40,
  },
});

export interface PledgeAgreementDataReact {
  pledgeNumber: string;
  pledgeDate: Date;
  client: {
    name: string;
    address?: string;
    city?: string;
    oib?: string;
  };
  item: {
    name: string;
    description: string;
    estimatedValue: number;
  };
  loanAmount: number;
  returnAmount: number;
  period: number;
  redeemDeadline: Date;
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

const PledgeAgreementDocument: React.FC<{ data: PledgeAgreementDataReact }> = ({ data }) => {
  return (
    <Document>
      <Page size="A4" style={styles.page}>
        {/* Header */}
        <View style={styles.header}>
          <View style={styles.companyBox}>
            <Text style={styles.companyTitle}>ZALAGAONICA:</Text>
            <Text style={styles.companyText}>PAWN SHOPS d.o.o.</Text>
            <Text style={styles.companyText}>P.J. Horvačanska cesta 25, Zagreb</Text>
            <Text style={styles.companyText}>Logorište 11a</Text>
            <Text style={styles.companyText}>47000 Karlovac</Text>
            <Text style={styles.companyText}>OIB: 51659874442</Text>
            <Text style={styles.companyText}>Tel: 092 500 8000</Text>
          </View>

          <View style={styles.clientBox}>
            <Text style={styles.clientTitle}>Zaloglitelj:</Text>
            <Text style={styles.clientText}>{data.client.name}</Text>
            {data.client.address && <Text style={styles.clientText}>{data.client.address}</Text>}
            {data.client.city && <Text style={styles.clientText}>{data.client.city}</Text>}
            {data.client.oib && <Text style={styles.clientText}>OIB: {data.client.oib}</Text>}
          </View>
        </View>

        {/* Metadata */}
        <View style={styles.metadata}>
          <Text>Broj ugovora: {data.pledgeNumber}</Text>
          <Text>Datum: {formatDate(data.pledgeDate)}</Text>
          <Text>Rok isplate: {formatDate(data.redeemDeadline)}</Text>
        </View>

        {/* Title */}
        <Text style={styles.title}>UGOVOR O ZALOGU - {data.pledgeNumber}</Text>

        {/* Introduction */}
        <Text style={styles.intro}>
          Ovaj ugovor o zalogu zaključen je između tvrtke PAWN SHOPS d.o.o. (dalje u tekstu: Zalagaonica) i gore
          navedenog zaloglitelja u skladu sa Zakonom o zalagaonicama i Općim uvjetima poslovanja.
        </Text>

        {/* Section 1: Pledged Item */}
        <Text style={styles.sectionTitle}>ČLANAK 1. - PREDMET ZALOGA</Text>
        <View style={styles.detailRow}>
          <Text style={styles.detailLabel}>Naziv predmeta:</Text>
          <Text style={styles.detailValue}>{data.item.name}</Text>
        </View>
        <View style={styles.detailRow}>
          <Text style={styles.detailLabel}>Opis i stanje:</Text>
          <Text style={styles.detailValue}>{data.item.description}</Text>
        </View>
        <View style={styles.detailRow}>
          <Text style={styles.detailLabel}>Procijenjena vrijednost:</Text>
          <Text style={styles.detailValue}>{formatCurrency(data.item.estimatedValue)}</Text>
        </View>

        {/* Section 2: Loan Terms */}
        <Text style={styles.sectionTitle}>ČLANAK 2. - UVJETI ZAJMA</Text>
        <View style={styles.detailRow}>
          <Text style={styles.detailLabel}>Iznos pozajmice:</Text>
          <Text style={styles.detailValue}>{formatCurrency(data.loanAmount)}</Text>
        </View>
        <View style={styles.detailRow}>
          <Text style={styles.detailLabel}>Iznos za isplatu:</Text>
          <Text style={styles.detailValue}>{formatCurrency(data.returnAmount)}</Text>
        </View>
        <View style={styles.detailRow}>
          <Text style={styles.detailLabel}>Razdoblje:</Text>
          <Text style={styles.detailValue}>{data.period} dana</Text>
        </View>
        <View style={styles.detailRow}>
          <Text style={styles.detailLabel}>Rok isplate:</Text>
          <Text style={styles.detailValue}>{formatDate(data.redeemDeadline)}</Text>
        </View>

        {/* Section 3: Terms and Conditions */}
        <Text style={styles.sectionTitle}>ČLANAK 3. - OPĆI UVJETI I PRAVA STRANAKA</Text>
        <Text style={styles.termItem}>
          1. Zaloglitelj izjavljuje da je isključivi vlasnik predmeta zaloga i da predmet nije opterećen pravima
          trećih osoba.
        </Text>
        <Text style={styles.termItem}>
          2. Zaloglitelj se obvezuje da će vratiti pozajmljeni iznos zajedno s kamatom u dogovorenom roku.
        </Text>
        <Text style={styles.termItem}>
          3. Zalagaonica se obvezuje da će čuvati predmet zaloga s dužnom pažnjom i osigurati ga od uništenja i
          oštećenja.
        </Text>
        <Text style={styles.termItem}>
          4. Zaloglitelj ima pravo otkupiti predmet zaloga u bilo kojem trenutku tijekom trajanja ugovora plaćanjem
          iznosa za isplatu.
        </Text>
        <Text style={styles.termItem}>
          5. U slučaju neisplate u dogovorenom roku, Zalagaonica stječe pravo prodaje predmeta zaloga radi naplate
          potraživanja.
        </Text>
        <Text style={styles.termItem}>
          6. Ako vrijednost prodaje predmeta prelazi iznos duga, razlika pripada zaloglitelju i može se isplatiti na
          zahtjev.
        </Text>
        <Text style={styles.termItem}>
          7. Ako vrijednost prodaje ne pokriva dug, Zalagaonica zadržava pravo naplate ostatka duga od zaloglitelja.
        </Text>
        <Text style={styles.termItem}>
          8. Zaloglitelj može produljiti rok isplate ugovorom o obnovi uz plaćanje dodatnih kamata.
        </Text>

        {/* Section 4: Special Provisions */}
        <Text style={styles.sectionTitle}>ČLANAK 4. - POSEBNE ODREDBE</Text>
        <Text style={styles.intro}>
          Zaloglitelj je upoznat s Općim uvjetima poslovanja Zalagaonice koji čine sastavni dio ovog ugovora.
          Zaloglitelj potvrđuje da je primio kopiju ovog ugovora te da su mu objašnjena sva prava i obveze.
        </Text>

        {/* Signatures */}
        <View style={styles.signatures}>
          <View style={styles.signatureBlock}>
            <Text style={styles.signatureLabel}>ZALAGAONICA:</Text>
            <Text style={styles.signatureLabel}>Izradio:</Text>
            <View style={styles.signatureLine} />
            <Text style={styles.signatureStamp}>m.p.</Text>
          </View>

          <View style={styles.signatureBlock}>
            <Text style={styles.signatureLabel}>ZALOGLITELJ:</Text>
            <Text style={styles.signatureLabel}>{data.client.name}</Text>
            <View style={styles.signatureLine} />
            <Text style={styles.signatureStamp}>(potpis)</Text>
          </View>
        </View>

        {/* Footer */}
        <Text style={styles.pageNumber}>1 od 1</Text>
        <Text style={styles.footer}>Datum ispisa: {formatDate(new Date())}</Text>
      </Page>
    </Document>
  );
};

export const generatePledgeAgreementReact = async (data: PledgeAgreementDataReact): Promise<void> => {
  const blob = await pdf(<PledgeAgreementDocument data={data} />).toBlob();
  const url = URL.createObjectURL(blob);
  const link = document.createElement('a');
  link.href = url;
  link.download = `ugovor-zalog-${data.pledgeNumber}.pdf`;
  link.click();
  URL.revokeObjectURL(url);
};
