import React from 'react';
import { useNavigate } from 'react-router-dom'; // << 1. Import za navigaciju

// << 2. Dodan ArrowLeftIcon za gumb za povratak
import { UserCircleIcon, ArrowLeftOnRectangleIcon, ArrowLeftIcon } from '@heroicons/react/24/outline';

interface HeaderProps {
    title: string;
    showBackButton?: boolean; // << 3. Dodajemo opcionalni prop
}

const Header: React.FC<HeaderProps> = ({ title, showBackButton = false }) => { // << 4. Preuzimamo novi prop
    const navigate = useNavigate(); // << 5. Logika za povratak
    
    const handleBack = () => {
        navigate(-1);
    };

    return (
        <header className="flex items-center justify-between h-16 px-8 bg-white border-b">
            {/* << 6. Novi div za grupiranje gumba i naslova */}
            <div className="flex items-center">
                {/* << 7. Uvjetno prikazivanje gumba za povratak */}
                {showBackButton && (
                    <button
                      onClick={handleBack}
                      className="mr-4 p-2 -ml-2 rounded-full hover:bg-gray-200 transition-colors"
                      aria-label="Natrag"
                    >
                        <ArrowLeftIcon className="h-6 w-6 text-gray-600" />
                    </button>
                )}
                <h2 className="text-2xl font-semibold text-gray-800">{title}</h2>
            </div>

            {/* Vaš postojeći kod za prikaz korisnika i odjavu ostaje netaknut */}
            <div className="flex items-center space-x-4">
                <div className="flex items-center">
                    <UserCircleIcon className="w-8 h-8 text-gray-600" />
                    <span className="ml-2 font-medium text-gray-700">Ime Korisnika</span>
                </div>
                <button className="p-2 rounded-full hover:bg-gray-200" title="Odjava">
                    <ArrowLeftOnRectangleIcon className="w-6 h-6 text-gray-600" />
                </button>
            </div>
        </header>
    );
};

export default Header;