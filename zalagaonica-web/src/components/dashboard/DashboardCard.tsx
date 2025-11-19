import React, { ElementType } from 'react';
import { Link } from 'react-router-dom';

interface DashboardCardProps {
  title: string;
  description: string;
  icon: ElementType;
  to?: string;
}

//                 ↓↓↓↓ 1. Preuzmite 'to' prop ovdje
const DashboardCard: React.FC<DashboardCardProps> = ({ title, description, icon: Icon, to }) => {
  
  // Sadržaj kartice je isti
  const cardContent = (
    <div className="bg-white p-6 rounded-lg shadow-sm hover:shadow-lg transition-shadow duration-300 cursor-pointer border border-gray-200 h-full flex flex-col">
      <div className="flex-shrink-0">
        <Icon className="h-8 w-8 text-indigo-600" />
      </div>
      <div className="mt-4 flex-grow">
        <h3 className="text-lg font-semibold text-gray-800">{title}</h3>
        <p className="mt-1 text-sm text-gray-500">{description}</p>
      </div>
    </div>
  );

  // 2. Ključna logika: ako postoji 'to' prop, koristi Link komponentu za navigaciju
  if (to) {
    return <Link to={to}>{cardContent}</Link>;
  }

  // Ako 'to' prop ne postoji, prikaži običan div koji nije link
  return cardContent;
};

export default DashboardCard;