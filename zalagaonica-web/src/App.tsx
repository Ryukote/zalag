import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';

import DashboardPage from './pages/DashboardPage';
import ClientPage from './pages/ClientPage';
import { PriceListPage } from './pages/PriceListPage';
import { IncomingDocumentsPage } from './pages/IncomingDocumentsPage';
import PledgeListPage from './pages/PledgeListPage';
import UnitOfMeasurePage from './pages/UnitOfMeasurePage';
import { OutputDocumentsPage } from './pages/OutputDocumentsPage';
import { CashRegisterPage } from './pages/CashRegisterPage';
import VehiclesPage from './pages/VehiclesPage';
import DeliveryCostsPage from './pages/DeliveryCostsPage';
import { WorkerVacationPage } from './pages/WorkerVacationPage';
import { AdminVacationPage } from './pages/AdminVacationPage';
import PayrollPage from './pages/PayrollPage';
import LoanPage from './pages/LoanPage';
import RepaymentPage from './pages/RepaymentPage';
import { ReportPageWrapper } from './pages/ReportPageWrapper';
import { LoginForm } from './pages/Auth';
import { InventoryProvider } from './context/InventoryContext';
import { ArticleListPage } from './pages/ArticleListPage';
import ImportCalculationPage from './pages/ImportCalculationPage';
import LabelsPage from './pages/LabelsPage';
import PriceChangeLogPage from './pages/PriceChangeLogPage';
import InventoryBookPage from './pages/InventoryBookPage';
import WarehouseCardsPage from './pages/WarehouseCardsPage';
import CustomerDebtsPage from './pages/CustomerDebtsPage';
import InventoryPage from './pages/InventoryPage';
import EndOfWorkPage from './pages/EndOfWorkPage';

function App() {
  return (
    <InventoryProvider>
      <Router>
        <Routes>
          <Route path="/" element={<DashboardPage />} />
          <Route path="/login" element={<LoginForm onLogin={(token) => {/* implementacija login */}} />} />
          <Route path="/komitenti" element={<ClientPage />} />
          <Route path="/artikli" element={<ArticleListPage />} />
          <Route path="/cjenik" element={<PriceListPage />} />
          <Route path="/ulazni-dokumenti" element={<IncomingDocumentsPage />} />
          <Route path="/izlazni-dokumenti" element={<OutputDocumentsPage />} />
          <Route path="/knjiga-zaloga" element={<PledgeListPage />} />
          <Route path="/jedinice-mjere" element={<UnitOfMeasurePage />} />
          <Route path="/blagajna" element={<CashRegisterPage />} />
          <Route path="/vozila" element={<VehiclesPage />} />
          <Route path="/dostava" element={<DeliveryCostsPage />} />
          <Route path="/godisnji-radnik" element={<WorkerVacationPage />} />
          <Route path="/godisnji-admin" element={<AdminVacationPage />} />
          <Route path="/place" element={<PayrollPage />} />
          <Route path="/zajmovi" element={<LoanPage />} />
          <Route path="/otplate" element={<RepaymentPage />} />
          <Route path="/ulazna-kalkulacija" element={<ImportCalculationPage />} />
          <Route path="/naljepnice" element={<LabelsPage />} />
          <Route path="/zapisnik-promjene-cijene" element={<PriceChangeLogPage />} />
          <Route path="/knjiga-popisa" element={<InventoryBookPage />} />
          <Route path="/skladiste-kartice" element={<WarehouseCardsPage />} />
          <Route path="/kupci-zaduzenja" element={<CustomerDebtsPage />} />
          <Route path="/inventura" element={<InventoryPage />} />
          <Route path="/kraj-rada" element={<EndOfWorkPage />} />
          <Route path="/reports/otkupni-blok/:id" element={<ReportPageWrapper type="otkupni-blok" />} />
          <Route path="/reports/racun-o-isplati/:id" element={<ReportPageWrapper type="racun-o-isplati" />} />
          <Route path="/reports/zahtjev-procjenu/:id" element={<ReportPageWrapper type="zahtjev-procjenu" />} />
          <Route path="/reports/medjuskladisnica/:id" element={<ReportPageWrapper type="medjuskladisnica" />} />
          <Route path="/reports/izvjesce-o-prodaji-artikla/:id" element={<ReportPageWrapper type="izvjesce-o-prodaji-artikla" />} />
        </Routes>
      </Router>
    </InventoryProvider>
  );
}

export default App;
