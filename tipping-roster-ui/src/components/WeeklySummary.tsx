import { useEffect, useState, useCallback } from "react";
import { getWeeklySummary } from "../api/client";
import type { WeeklySummary as Summary } from "../types/models";

const euro = new Intl.NumberFormat("en-US", {
    style: "currency",
    currency: "EUR",
    minimumFractionDigits: 2,
});

export default function WeeklySummary() {
    const [summary, setSummary] = useState<Summary | null>(null);

    const fetchData = useCallback(async () => {
        try {
            const data = await getWeeklySummary();
            setSummary(data);
        } catch {
            setSummary(null);
        }
    }, []);

    useEffect(() => {
        let mounted = true;
        fetchData();

        const onCreated = () => {
            if (mounted) fetchData();
        };

        window.addEventListener("shiftCreated", onCreated);
        return () => {
            mounted = false;
            window.removeEventListener("shiftCreated", onCreated);
        };
    }, [fetchData]);

    if (!summary) return <p className="loading">Loading summary...</p>;

    return (
        <div className="centered-container">
            <section className="card summary-card">
                <div className="summary-header">
                    <h2>Weekly Tips</h2>
                    <div className="summary-total">{euro.format(summary.totalTips)}</div>
                </div>

                <div className="table-wrap">
                    <table className="styled-table centered-table">
                        <thead>
                            <tr>
                                <th>Employee</th>
                                <th className="numeric">Hours</th>
                                <th className="numeric">Tip Share</th>
                            </tr>
                        </thead>
                        <tbody>
                            {summary.employees.map(e => (
                                <tr key={e.employeeId}>
                                    <td className="employee-cell">
                                        <div className="emp-info">
                                            <div className="emp-name">{e.name}</div>
                                        </div>
                                    </td>
                                    <td className="numeric">{e.hoursWorked.toFixed(2)}</td>
                                    <td className="numeric">{euro.format(e.tipAmount)}</td>
                                </tr>
                            ))}
                        </tbody>
                    </table>
                </div>
            </section>
        </div>
    );
}