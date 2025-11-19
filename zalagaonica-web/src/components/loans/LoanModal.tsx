import React, { useState, useEffect } from "react";

const LoanModal = ({ loan, onSave, onClose }: any) => {
  const [clientName, setClientName] = useState("");
  const [date, setDate] = useState("");
  const [amount, setAmount] = useState(0);
  const [interest, setInterest] = useState(0);
  const [dueDate, setDueDate] = useState("");

  useEffect(() => {
    if (loan) {
      setClientName(loan.clientName);
      setDate(loan.date.split("T")[0]);
      setAmount(loan.amount);
      setInterest(loan.interest);
      setDueDate(loan.dueDate.split("T")[0]);
    } else {
      setClientName("");
      setDate("");
      setAmount(0);
      setInterest(0);
      setDueDate("");
    }
  }, [loan]);

  const handleSubmit = () => {
    if (!clientName || !date || amount <= 0 || interest < 0 || !dueDate) {
      alert("Please fill all fields with valid data.");
      return;
    }
    onSave({
      id: loan ? loan.id : undefined,
      clientName,
      date,
      amount,
      interest,
      dueDate,
    });
  };

  return (
    <div className="fixed inset-0 bg-black bg-opacity-50 flex justify-center items-center z-50">
      <div className="bg-white rounded p-6 w-96 shadow-md">
        <h2 className="text-lg font-semibold mb-4">{loan ? "Edit Loan" : "Add Loan"}</h2>
        <div className="mb-3">
          <label className="block mb-1">Client Name</label>
          <input
            type="text"
            value={clientName}
            onChange={(e) => setClientName(e.target.value)}
            className="w-full border border-gray-300 rounded px-2 py-1"
          />
        </div>

        <div className="mb-3">
          <label className="block mb-1">Date</label>
          <input
            type="date"
            value={date}
            onChange={(e) => setDate(e.target.value)}
            className="w-full border border-gray-300 rounded px-2 py-1"
          />
        </div>

        <div className="mb-3">
          <label className="block mb-1">Amount</label>
          <input
            type="number"
            min={0}
            value={amount}
            onChange={(e) => setAmount(parseFloat(e.target.value))}
            className="w-full border border-gray-300 rounded px-2 py-1"
          />
        </div>

        <div className="mb-3">
          <label className="block mb-1">Interest (%)</label>
          <input
            type="number"
            min={0}
            step={0.01}
            value={interest}
            onChange={(e) => setInterest(parseFloat(e.target.value))}
            className="w-full border border-gray-300 rounded px-2 py-1"
          />
        </div>

        <div className="mb-4">
          <label className="block mb-1">Due Date</label>
          <input
            type="date"
            value={dueDate}
            onChange={(e) => setDueDate(e.target.value)}
            className="w-full border border-gray-300 rounded px-2 py-1"
          />
        </div>

        <div className="flex justify-end space-x-2">
          <button
            onClick={onClose}
            className="px-4 py-2 bg-gray-300 hover:bg-gray-400 rounded"
          >
            Cancel
          </button>
          <button
            onClick={handleSubmit}
            className="px-4 py-2 bg-blue-600 hover:bg-blue-700 text-white rounded"
          >
            Save
          </button>
        </div>
      </div>
    </div>
  );
};

export default LoanModal;
