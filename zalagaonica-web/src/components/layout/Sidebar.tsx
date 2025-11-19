// src/components/layout/Sidebar.tsx
import React from 'react';
import { 
  HomeIcon, ChartBarIcon, UsersIcon, Cog6ToothIcon, BuildingStorefrontIcon, TruckIcon, CircleStackIcon
} from '@heroicons/react/24/outline';

// Mapiranje starih stavki menija na nove s ikonama
const navigation = [
  { name: 'Nadzorna ploča', href: '#', icon: HomeIcon },
  { name: 'Artikli i cjenici', href: '#', icon: CircleStackIcon },
  { name: 'Dokumenti', href: '#', icon: TruckIcon },
  { name: 'Partneri', href: '#', icon: UsersIcon },
  { name: 'Izvješća', href: '#', icon: ChartBarIcon },
  { name: 'Postavke', href: '#', icon: Cog6ToothIcon },
];

const Sidebar: React.FC = () => {
  return (
    <aside className="flex flex-col w-64 bg-white shadow-md">
      <div className="h-16 flex items-center justify-center border-b">
        {/* Ovdje ide vaš logo */}
        <h1 className="text-xl font-bold text-blue-600">PoslovanjeApp</h1>
      </div>
      <nav className="flex-1 px-4 py-4 space-y-2">
        {navigation.map((item) => (
          <a
            key={item.name}
            href={item.href}
            className="flex items-center px-4 py-2 text-gray-700 rounded-lg hover:bg-blue-50 hover:text-blue-600 transition-colors"
          >
            <item.icon className="w-6 h-6 mr-3" />
            {item.name}
          </a>
        ))}
      </nav>
      <div className="p-4 border-t">
        <p className="text-xs text-gray-500">Verzija: 2025.1.0</p>
      </div>
    </aside>
  );
};

export default Sidebar;