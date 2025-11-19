import React from 'react';

interface Props {
  text: string;
}

const PulsingLoader: React.FC<Props> = ({ text }) => {
  return (
    <div className="flex flex-col items-center justify-center text-center p-4">
      <div className="relative flex justify-center items-center">
        <div className="absolute w-16 h-16 rounded-full border-4 border-blue-500 animate-ping"></div>
        <div className="w-12 h-12 bg-blue-500 rounded-full"></div>
      </div>
      <p className="mt-4 text-lg font-semibold text-gray-700">{text}</p>
    </div>
  );
};

export default PulsingLoader;