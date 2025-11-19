// src/components/dashboard/InfoPanel.tsx
import React from 'react';

interface InfoPanelProps {
  companyName: string;
  vatSystem: string;
  businessYear: number;
  warehouse: string;
  businessUnit: string;
}

const InfoPanel: React.FC<InfoPanelProps> = ({ companyName, vatSystem, businessYear, warehouse, businessUnit }) => {
  return (
    <div className="p-4 bg-white rounded-lg shadow-sm border border-gray-200">
      <div className="grid grid-cols-2 md:grid-cols-4 gap-4">
        <div>
          <p className="text-sm font-medium text-gray-500">Tvrtka</p>
          <p className="text-lg font-semibold text-gray-800">{companyName}</p>
          <p className="text-sm text-green-600">{vatSystem}</p>
        </div>
        <div>
          <p className="text-sm font-medium text-gray-500">Poslovna godina</p>
          <p className="text-lg font-semibold text-gray-800">{businessYear}</p>
        </div>
        <div>
          <p className="text-sm font-medium text-gray-500">Skladište</p>
          <p className="text-lg font-semibold text-gray-800">{warehouse}</p>
        </div>
        <div>
          <p className="text-sm font-medium text-gray-500">Poslovni prostor</p>
          <p className="text-lg font-semibold text-gray-800">{businessUnit}</p>
        </div>
      </div>
    </div>
  );
};

export default InfoPanel;