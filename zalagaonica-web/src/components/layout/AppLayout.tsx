// src/components/layout/AppLayout.tsx
import React from 'react';

export const AppLayout: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  return (
    <div className="min-h-screen bg-gray-100 text-gray-900">
      {/* Ovo može biti sidebar/header itd. */}
      <main>{children}</main>
    </div>
  );
};

export default AppLayout;
