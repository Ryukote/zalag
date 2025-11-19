import React, { useState, useEffect } from "react";

const RepaymentModal = ({ repayment, onSave, onClose }: any) => {
  const [loanId, setLoanId] = useState("");
  const [date, setDate] = useState("");
  const [amount, setAmount] = useState(0);

  useEffect(() => {
    if (repayment) {
      setLoanId(repayment.loanId);
      setDate(repayment.date.split("T")[0]);
      setAmount(repayment.amount);
    } else {
      setLoanId("");
      setDate("");
      setAmount(0);
    }
  }, [repayment]);

  const handleSubmit = () => {
    if (!loanId || !date || amount <= 0) {
      alert("Please fill all fields with valid data.");
      return;
    }
    onSave({
      id: repayment ? repayment.id : undefined,
      loanId,
      date,
      amount,
    });
  };

  return (
    <div className="fixed inset-0 bg-black bg-opacity-50 flex justify-center items-center z-50">
      <div className="bg-white rounded p-6 w-96 shadow-md">
        <h2 className="text-lg font-semibold mb-4">{repayment ? "Edit Repayment" : "Add Repayment"}</h2>
        <div className="mb-3">
          <label className="block mb-1">Loan ID</label>
          <input
            type="text"
            value={loanId}
            onChange={(e) => setLoanId(e.target.value)}
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

        <div className="mb-4">
          <label className="block mb-1">Amount</label>
          <input
            type="number"
            min={0}
            value={amount}
            onChange={(e) => setAmount(parseFloat(e.target.value))}
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
            className="px-4 py-2 bg-green-600 hover:bg-green-700 text-white rounded"
          >
            Save
          </button>
        </div>
      </div>
    </div>
  );
};

export default RepaymentModal;
