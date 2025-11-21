import React, { useState, useRef } from 'react';
import AppLayout from '../components/layout/AppLayout';
import Header from '../components/layout/Header';
import DashboardCard from '../components/dashboard/DashboardCard';
import InfoPanel from '../components/dashboard/InfoPanel';
import {
  ChevronDownIcon,
  ArrowDownOnSquareIcon,
  CalculatorIcon,
  ArrowUpOnSquareIcon,
  TagIcon,
  BuildingStorefrontIcon,
  TicketIcon,
  PencilSquareIcon,
  BookOpenIcon,
  RectangleStackIcon,
  UsersIcon,
  ArchiveBoxIcon,
  PowerIcon,
  CreditCardIcon,
  BanknotesIcon,
  BuildingLibraryIcon,
  ScaleIcon,
  FolderIcon,
  HomeModernIcon,
  DocumentDuplicateIcon,
  ChatBubbleBottomCenterTextIcon,
  MagnifyingGlassIcon,
  DocumentMagnifyingGlassIcon,
  TableCellsIcon,
  ChartPieIcon,
  ChartBarIcon,
  DocumentChartBarIcon,
  UserGroupIcon,
  ClipboardDocumentListIcon,
  WrenchScrewdriverIcon,
  ClipboardDocumentCheckIcon,
  ShoppingCartIcon,
  QueueListIcon,
  ReceiptPercentIcon,
  CogIcon,
  ChartBarSquareIcon,
  Cog8ToothIcon,
  PencilIcon,
  AdjustmentsHorizontalIcon,
  ArchiveBoxXMarkIcon,
  KeyIcon,
  EnvelopeIcon,
  UserCircleIcon,
  CloudArrowUpIcon,
  InformationCircleIcon,
  WrenchIcon,
  Bars3Icon,
  CurrencyDollarIcon,
  TruckIcon,
} from '@heroicons/react/24/outline';

// Definicija tipa za navigacijsku stavku
interface NavItem {
  label: string;
  icon: React.ElementType;
  href?: string;
  isSeparator?: boolean;
}

// Definicija tipa za glavnu kategoriju navigacije
interface NavCategory {
  title: string;
  items: NavItem[];
}

// Podaci za navigaciju
const navigationData: NavCategory[] = [
    {
    title: 'Podaci',
    items: [
      { label: 'Ulazni dokumenti', icon: ArrowDownOnSquareIcon, href: '/ulazni-dokumenti' },
      { label: 'Ulazna kalkulacija uvoza', icon: CalculatorIcon, href: '/ulazna-kalkulacija' },
      { label: 'Izlazni dokumenti', icon: ArrowUpOnSquareIcon, href: '/izlazni-dokumenti' },
      { isSeparator: true, label: '', icon: () => null },
      { label: 'Artikli', icon: TagIcon, href: '/artikli' },
      { label: 'Cjenik - skladište', icon: BuildingStorefrontIcon, href: '/cjenik' },
      { label: 'Naljepnice', icon: TicketIcon, href: '/naljepnice' },
      { isSeparator: true, label: '', icon: () => null },
      { label: 'Zapisnik o promjeni cijene', icon: PencilSquareIcon, href: '/zapisnik-promjene-cijene' },
      { label: 'Knjiga popisa (Dnevnik maloprodaje)', icon: BookOpenIcon, href: '/knjiga-popisa' },
      { isSeparator: true, label: '', icon: () => null },
      { label: 'Skladište - Kartice', icon: RectangleStackIcon, href: '/skladiste-kartice' },
      { label: 'Kupci - zaduženja', icon: UsersIcon, href: '/kupci-zaduzenja' },
      { label: 'Inventura', icon: ArchiveBoxIcon, href: '/inventura' },
      { isSeparator: true, label: '', icon: () => null },
      { label: 'Kraj rada', icon: PowerIcon, href: '/kraj-rada' },
    ],
  },
  {
    title: 'Pomoćni podaci',
    items: [
      { label: 'Komitenti', icon: UsersIcon, href: '/komitenti' },
      { label: 'Načini plaćanja', icon: CreditCardIcon, href: '#' },
      { label: 'Porezne grupe', icon: BanknotesIcon, href: '#' },
      { label: 'Blagajna', icon: BuildingLibraryIcon, href: '/blagajna' },
      { isSeparator: true, label: '', icon: () => null },
      { label: 'Jedinice mjere', icon: ScaleIcon, href: '#' },
      { label: 'Vrste artikala - grupe', icon: FolderIcon, href: '#' },
      { label: 'Artikli', icon: TagIcon, href: '#' },
      { isSeparator: true, label: '', icon: () => null },
      { label: 'Tipovi skladišta', icon: HomeModernIcon, href: '#' },
      { label: 'Skladišta', icon: BuildingStorefrontIcon, href: '#' },
      { isSeparator: true, label: '', icon: () => null },
      { label: 'Dokumenti', icon: DocumentDuplicateIcon, href: '#' },
      { label: 'Napomene izlaznih dok.', icon: ChatBubbleBottomCenterTextIcon, href: '#' },
    ],
  },
    {
    title: 'Knjiga Zaloga',
    items: [
      { label: 'Pregled zaloga', icon: ClipboardDocumentListIcon, href: '/knjiga-zaloga' },
    ],
  },
  {
    title: 'Pretraživanje',
    items: [
      { label: '🔍 Centralno pretraživanje dokumenata', icon: DocumentMagnifyingGlassIcon, href: '/pretraga-dokumenata' },
      { isSeparator: true, label: '', icon: () => null },
      { label: 'Pretraživanje artikala', icon: MagnifyingGlassIcon, href: '#' },
      { label: 'Pretraživanje artikala po lot broju', icon: DocumentMagnifyingGlassIcon, href: '#' },
      { isSeparator: true, label: '', icon: () => null },
      { label: 'Ulaznih dokumenata', icon: ArrowDownOnSquareIcon, href: '#' },
      { label: 'Izlaznih dokumenata', icon: ArrowUpOnSquareIcon, href: '#' },
    ],
  },
  {
    title: 'Izvješća',
    items: [
      { label: '📊 Admin nadzorna ploča', icon: ChartBarSquareIcon, href: '/admin-dashboard' },
      { isSeparator: true, label: '', icon: () => null },
      { label: 'Cjenik', icon: TableCellsIcon, href: '#' },
      { label: 'Stanje skladišta', icon: ChartPieIcon, href: '#' },
      { label: 'Trenutno stanje skladišta', icon: ChartBarIcon, href: '#' },
      { isSeparator: true, label: '', icon: () => null },
      { label: 'Rekapitulacija ulaza - po dokumentima', icon: DocumentChartBarIcon, href: '#' },
      { label: 'Rekapitulacija po dobavljačima', icon: UserGroupIcon, href: '#' },
      { isSeparator: true, label: '', icon: () => null },
      { label: 'Rekapitulacija izlaza - po dokumentima', icon: DocumentChartBarIcon, href: '#' },
      { label: 'Rekapitulacija zapisnika o pr. cjene', icon: ClipboardDocumentListIcon, href: '#' },
      { isSeparator: true, label: '', icon: () => null },
      { label: 'Proizvodnja - garancije', icon: WrenchScrewdriverIcon, href: '#' },
      { label: 'Knjiga popisa - izvještaj (rekapitulacija)', icon: ClipboardDocumentCheckIcon, href: '#' },
    ],
  },
  {
    title: 'Maloprodaja',
    items: [
        { label: 'Knjiga popisa (Dnevnik maloprodaje)', icon: BookOpenIcon, href: '#' },
        { label: 'Knjiga popisa - izvještaj (rekapitulacija)', icon: ClipboardDocumentCheckIcon, href: '#' },
        { label: 'Promet maloprodaje - rekapitulacija', icon: ShoppingCartIcon, href: '#' },
        { label: 'Blagajna', icon: BuildingLibraryIcon, href: '/blagajna' },
        { isSeparator: true, label: '', icon: () => null },
        { label: 'Knjiženje prometa maloprodajne kase', icon: QueueListIcon, href: '#' },
        { label: 'Pregled prodaje maloprodajne blagajne', icon: ReceiptPercentIcon, href: '#' },
    ]
  },
  {
      title: 'Proizvodnja',
      items: [
          { label: 'Proizvodnja - pregled', icon: CogIcon, href: '#' },
      ]
  },
  {
      title: 'Analitika',
      items: [
          { label: 'Pregled prodaje po kupcima i grupama', icon: ChartBarSquareIcon, href: '#' },
      ]
  },
  {
    title: 'Podešavanja',
    items: [
      { label: 'Opcije programa...', icon: Cog8ToothIcon, href: '#' },
      { isSeparator: true, label: '', icon: () => null },
      { label: 'Izmjena - knjiga popisa', icon: PencilIcon, href: '#' },
      { label: 'Izmjena cjenika', icon: PencilSquareIcon, href: '#' },
      { label: 'Parametri aplikacije', icon: AdjustmentsHorizontalIcon, href: '#' },
      { label: 'Inventura po grupi', icon: ArchiveBoxXMarkIcon, href: '#' },
      { isSeparator: true, label: '', icon: () => null },
      { label: 'Autorizacija', icon: KeyIcon, href: '#' },
      { label: 'Testiranje mail-a', icon: EnvelopeIcon, href: '#' },
      { label: 'Korisnik', icon: UserCircleIcon, href: '#' },
      { label: 'Eracun', icon: CloudArrowUpIcon, href: '#' },
    ],
  },
  {
    title: 'Pomoć',
    items: [
      { label: 'O programu', icon: InformationCircleIcon, href: '#' },
      { label: 'Postavke programa...', icon: WrenchIcon, href: '#' },
    ],
  },
];

// Komponenta za padajući izbornik
const NavDropdown: React.FC<{ 
  category: NavCategory;
  isOpen: boolean;
  onMouseEnter: () => void;
}> = ({ category, isOpen, onMouseEnter }) => {
  return (
    <div 
      className="relative"
      onMouseEnter={onMouseEnter}
    >
      <button
        className="inline-flex items-center px-4 py-2 text-sm font-medium text-gray-200 bg-gray-800 rounded-md hover:bg-gray-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-offset-gray-800 focus:ring-white transition-colors duration-150"
      >
        <span>{category.title}</span>
        <ChevronDownIcon className="w-4 h-4 ml-2 -mr-1" />
      </button>

      {isOpen && (
        <div 
          className="origin-top-right absolute left-0 mt-2 w-64 rounded-md shadow-lg bg-white ring-1 ring-black ring-opacity-5 z-20"
        >
          <div className="py-1" role="menu" aria-orientation="vertical" aria-labelledby="options-menu">
            {category.items.map((item, index) => {
              if (item.isSeparator) {
                return <div key={`separator-${index}`} className="border-t border-gray-200 my-1" />;
              }
              const Icon = item.icon;
              return (
                <a
                  key={item.label}
                  href={item.href || '#'}
                  className="flex items-center px-4 py-2 text-sm text-gray-700 hover:bg-gray-100 hover:text-gray-900"
                  role="menuitem"
                >
                  <Icon className="w-5 h-5 mr-3 text-gray-500" />
                  <span>{item.label}</span>
                </a>
              );
            })}
          </div>
        </div>
      )}
    </div>
  );
};

// Glavna komponenta navigacijske trake
const ModernNavbar: React.FC = () => {
  const [isMobileMenuOpen, setIsMobileMenuOpen] = useState(false);
  const [openDropdown, setOpenDropdown] = useState<string | null>(null);

  return (
    <nav className="bg-gray-800 shadow-md">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div className="flex items-center justify-between h-16">
          <div className="flex-shrink-0">
            <span className="text-white font-bold text-xl">Zalagaonica</span>
          </div>
          <div className="hidden md:block">
            <div 
              className="ml-10 flex items-baseline space-x-2"
              onMouseLeave={() => setOpenDropdown(null)}
            >
              {navigationData.map((category) => (
                <NavDropdown 
                  key={category.title} 
                  category={category}
                  isOpen={openDropdown === category.title}
                  onMouseEnter={() => setOpenDropdown(category.title)}
                />
              ))}
            </div>
          </div>
          <div className="-mr-2 flex md:hidden">
            <button
              onClick={() => setIsMobileMenuOpen(!isMobileMenuOpen)}
              className="bg-gray-800 inline-flex items-center justify-center p-2 rounded-md text-gray-400 hover:text-white hover:bg-gray-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-offset-gray-800 focus:ring-white"
            >
              <span className="sr-only">Otvori glavni izbornik</span>
              <Bars3Icon className="block h-6 w-6" aria-hidden="true" />
            </button>
          </div>
        </div>
      </div>

      {isMobileMenuOpen && (
        <div className="md:hidden">
          <div className="px-2 pt-2 pb-3 space-y-1 sm:px-3">
            {navigationData.map((category) => (
              <div key={category.title} className="text-white">
                <h3 className="px-3 py-2 text-xs font-bold uppercase tracking-wider text-gray-400">{category.title}</h3>
                {category.items.map((item) => {
                   if (item.isSeparator) return null;
                   const Icon = item.icon;
                   return (
                     <a
                       key={item.label}
                       href={item.href || '#'}
                       className="flex items-center rounded-md px-3 py-2 text-base font-medium text-gray-300 hover:bg-gray-700 hover:text-white"
                     >
                       <Icon className="w-5 h-5 mr-3" />
                       {item.label}
                     </a>
                   );
                })}
              </div>
            ))}
          </div>
        </div>
      )}
    </nav>
  );
};


const DashboardPage: React.FC = () => {
  const companyInfo = {
    companyName: 'PAWN SHOPS d.o.o.',
    vatSystem: 'U sustavu PDV-a',
    businessYear: 2025,
    warehouse: 'Zalagaonica (ZG3)',
    businessUnit: 'P6',
  };

  return (
    <AppLayout>
      {/* Uključujemo navigaciju direktno ovdje radi cjelovitosti koda */}
      <ModernNavbar /> 
      <div className="bg-gray-50 min-h-screen px-6 py-8">
        {/* Header */}
        <Header title="Nadzorna ploča" />

        {/* Gornji info panel */}
        <div className="mt-6">
          <InfoPanel {...companyInfo} />
        </div>

        {/* Brze akcije */}
        <section className="mt-10">
          <h2 className="text-xl font-semibold text-gray-700 mb-4">Brze akcije</h2>
          <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-6">
            <DashboardCard
              title="Artikli"
              icon={TagIcon}
              description="Pregled i uređivanje artikala"
              to="/artikli"
            />
            <DashboardCard
              title="Ulazni dokumenti"
              icon={ArrowDownOnSquareIcon}
              description="Primke, kalkulacije..."
              to="/ulazni-dokumenti"
            />
            <DashboardCard
              title="Izlazni dokumenti"
              icon={ArrowUpOnSquareIcon}
              description="Računi, otpremnice..."
              to="/izlazni-dokumenti"
            />
            <DashboardCard
              title="Komitenti"
              icon={UsersIcon}
              description="Upravljanje partnerima"
              to="/komitenti" 
            />
            <DashboardCard
              title="Cjenik i skladište"
              icon={CurrencyDollarIcon}
              description="Promjene cijena i stanja"
              to="/cjenik"
            />
            <DashboardCard
              title="Knjiga popisa"
              icon={BookOpenIcon}
              description="Inventura i zapisnici"
            />
            {/* --- NOVA KARTICA --- */}
            <DashboardCard
              title="Knjiga Zaloga"
              icon={ClipboardDocumentListIcon}
              description="Pregled i upravljanje zalozima"
              to="/knjiga-zaloga" // Link na novu stranicu
            />
            <DashboardCard
              title="Jedinice Mjere"
              icon={ScaleIcon}
              description="Upravljanje jedinicama mjere"
              to="/jedinice-mjere"
            />
            <DashboardCard
              title="Blagajna"
              icon={BanknotesIcon}
              description="Upravljanje gotovinskim transakcijama"
              to="/blagajna"
            />
            <DashboardCard
              title="Evidencija vozila"
              icon={TruckIcon}
              description="Upravljanje evidencijom vozila"
              to="/vozila"
            />
            <DashboardCard
              title="Troškovi dostave"
              icon={TruckIcon}
              description="Troškovi dostave"
              to="/dostava"
            />
            <DashboardCard
              title="Godišnji odmori"
              icon={TruckIcon}
              description="Godišnji odmori"
              to="/godisnji"
            />
            <DashboardCard
              title="Plaće zaposlenika"
              icon={CurrencyDollarIcon}
              description="Plaće zaposlenika"
              to="/place"
            />
          </div>
        </section>
      </div>
    </AppLayout>
  );
};

export default DashboardPage;
