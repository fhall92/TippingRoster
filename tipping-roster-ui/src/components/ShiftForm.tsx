import { useEffect, useState } from "react";
import { createShift, getEmployees } from "../api/client";
import type { CreateShiftRequest, Employee } from "../types/models";

export default function ShiftForm() {
    const [employees, setEmployees] = useState<Employee[]>([]);
    const [employeeId, setEmployeeId] = useState("");
    const [date, setDate] = useState("");
    const [startTime, setStartTime] = useState("");
    const [endTime, setEndTime] = useState("");
    const [error, setError] = useState<string | null>(null);
    const [isSubmitting, setIsSubmitting] = useState(false);

    useEffect(() => {
        let mounted = true;
        getEmployees()
            .then(list => {
                if (!mounted) return;
                setEmployees(list || []);
                if (list && list.length > 0) {
                    setEmployeeId(prev => prev || list[0].id);
                }
            })
            .catch(() => {
                if (!mounted) return;
                setEmployees([]);
            });
        return () => { mounted = false; };
    }, []);

    const selectedEmployee = employees.find(e => e.id === employeeId);

    const isTimeRangeValid = (): boolean => {
        if (!date || !startTime || !endTime) return true;
        const start = new Date(`${date}T${startTime}:00`);
        const end = new Date(`${date}T${endTime}:00`);
        return end > start;
    };

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();

        if (!employeeId) {
            setError("Please select an employee.");
            return;
        }

        if (!isTimeRangeValid()) {
            setError("End time must be after start time.");
            return;
        }

        setIsSubmitting(true);
        setError(null);

        const request: CreateShiftRequest = {
            employeeId,
            date,
            startTime,
            endTime,
        };

        try {
            await createShift(request);

            // Reset form
            setEmployeeId(employees.length > 0 ? employees[0].id : "");
            setDate("");
            setStartTime("");
            setEndTime("");

            // Notify listeners (RosterTable) to refresh — lightweight, avoids full page reload
            window.dispatchEvent(new CustomEvent("shiftCreated"));
        } catch {
            setError("Failed to create shift.");
        } finally {
            setIsSubmitting(false);
        }
    };

    const isInvalid = !isTimeRangeValid();

    return (
        <div className="centered-container">
            <form onSubmit={handleSubmit} className="shift-form card" aria-label="Add shift">
                <h2>Add Shift</h2>

                <div className="form-field full">
                    <label className="label">Employee</label>
                    <div className="select-row">
                        <div className="select-wrapper" style={{ width: "100%" }}>
                            <select
                                className="select"
                                value={employeeId}
                                onChange={(e) => setEmployeeId(e.target.value)}
                                required
                                aria-label="Select employee"
                            >
                                <option value="" disabled>Select employee</option>
                                {employees.map(emp => (
                                    <option key={emp.id} value={emp.id}>
                                        {emp.name}
                                    </option>
                                ))}
                            </select>
                        </div>
                    </div>
                    {selectedEmployee && <div className="employee-hint">Selected: <strong>{selectedEmployee.name}</strong></div>}
                </div>

                <div className="form-field">
                    <label className="label">Date</label>
                    <input
                        type="date"
                        className="input"
                        value={date}
                        onChange={(e) => setDate(e.target.value)}
                        required
                    />
                </div>

                <div className="form-field">
                    <label className="label">Start time</label>
                    <input
                        type="time"
                        className="input"
                        value={startTime}
                        onChange={(e) => setStartTime(e.target.value)}
                        required
                    />
                </div>

                <div className="form-field">
                    <label className="label">End time</label>
                    <input
                        type="time"
                        className="input"
                        value={endTime}
                        onChange={(e) => setEndTime(e.target.value)}
                        required
                    />
                </div>

                {error && <p className="form-error">{error}</p>}

                <button type="submit" className="btn" disabled={isInvalid || isSubmitting}>
                    {isSubmitting ? "Saving..." : "Add Shift"}
                </button>
            </form>
        </div>
    );
}