import React from 'react';
import { Listbox, Transition } from '@headlessui/react';
import {
  ArrowLongLeftIcon,
  ArrowLongRightIcon,
  ChevronUpDownIcon,
  CheckIcon,
} from '@heroicons/react/20/solid';

interface PaginationProps {
  currentPage: number;
  totalCount: number;
  itemsPerPage: number;
  onPageChange: (page: number) => void;
  onItemsPerPageChange: (value: number) => void;
}

export const Pagination: React.FC<PaginationProps> = ({ currentPage, totalCount, itemsPerPage, onPageChange, onItemsPerPageChange }) => {
  const totalPages = Math.ceil(totalCount / itemsPerPage);
  const firstItem = (currentPage - 1) * itemsPerPage + 1;
  const lastItem = Math.min(currentPage * itemsPerPage, totalCount);
  const pageOptions = [5, 10, 20, 50];

  if (totalCount === 0) return null;

  return (
    <nav className="flex items-center justify-between border-t border-gray-200 px-4 sm:px-0 mt-4">
      {/* Lijeva strana: Dropdown i info tekst */}
      <div className="flex items-center gap-x-4">
        
        <Listbox value={itemsPerPage} onChange={onItemsPerPageChange}>
          <div className="relative">
            <Listbox.Button className="relative w-full cursor-default rounded-md bg-white py-1.5 pl-3 pr-10 text-left text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 focus:outline-none focus:ring-2 focus:ring-indigo-600 sm:text-sm sm:leading-6">
              <span className="block truncate">{itemsPerPage}</span>
              <span className="pointer-events-none absolute inset-y-0 right-0 flex items-center pr-2">
                <ChevronUpDownIcon className="h-5 w-5 text-gray-400" aria-hidden="true" />
              </span>
            </Listbox.Button>
            <Transition
              as={React.Fragment}
              leave="transition ease-in duration-100"
              leaveFrom="opacity-100"
              leaveTo="opacity-0"
            >
              {/* <<< IZMJENA JE U OVOJ LINIJI ISPOD: Uklonjene su klase 'bottom-full' i 'mb-2' */}
              <Listbox.Options className="absolute z-10 mt-1 max-h-60 w-max overflow-auto rounded-md bg-white py-1 text-base shadow-lg ring-1 ring-black ring-opacity-5 focus:outline-none sm:text-sm">
                {pageOptions.map((option) => (
                  <Listbox.Option
                    key={option}
                    className={({ active }) =>
                      `relative cursor-default select-none py-2 pl-10 pr-4 ${
                        active ? 'bg-indigo-600 text-white' : 'text-gray-900'
                      }`
                    }
                    value={option}
                  >
                    {({ selected }) => (
                      <>
                        <span className={`block truncate ${selected ? 'font-medium' : 'font-normal'}`}>
                          {option}
                        </span>
                        {selected ? (
                          <span className="absolute inset-y-0 left-0 flex items-center pl-3 text-indigo-600">
                            <CheckIcon className="h-5 w-5" aria-hidden="true" />
                          </span>
                        ) : null}
                      </>
                    )}
                  </Listbox.Option>
                ))}
              </Listbox.Options>
            </Transition>
          </div>
        </Listbox>

        <p className="hidden md:block text-sm text-gray-700">
           Prikaz <span className="font-medium">{firstItem}</span>-<span className="font-medium">{lastItem}</span> od <span className="font-medium">{totalCount}</span>
        </p>
      </div>

      {/* Desna strana ostaje ista */}
      <div className="flex items-center gap-x-2">
        <button
          onClick={() => onPageChange(currentPage - 1)}
          disabled={currentPage === 1}
          className="inline-flex items-center rounded-md px-3 py-2 text-sm font-semibold text-gray-900 ring-1 ring-inset ring-gray-300 hover:bg-gray-50 disabled:text-gray-300 disabled:cursor-not-allowed"
        >
          <ArrowLongLeftIcon className="h-5 w-5 text-gray-400" aria-hidden="true" />
          <span className="ml-2">Prethodna</span>
        </button>
        <button
          onClick={() => onPageChange(currentPage + 1)}
          disabled={currentPage === totalPages}
          className="inline-flex items-center rounded-md px-3 py-2 text-sm font-semibold text-gray-900 ring-1 ring-inset ring-gray-300 hover:bg-gray-50 disabled:text-gray-300 disabled:cursor-not-allowed"
        >
          <span className="mr-2">Sljedeća</span>
          <ArrowLongRightIcon className="h-5 w-5 text-gray-400" aria-hidden="true" />
        </button>
      </div>
    </nav>
  );
};