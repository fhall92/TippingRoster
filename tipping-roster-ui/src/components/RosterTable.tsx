import { useEffect, useState, useCallback } from "react";
import { getWeeklyShifts, getEmployees } from "../api/client";
import type { Shift, Employee } from "../types/models";

export default function RosterTable() {
    const [shifts, setShifts] = useState<Shift[]>([]);
    const [employeeMap, setEmployeeMap] = useState<Record<string, Employee>>({});

    const fetchData = useCallback(async () => {
        try {
            const [shiftsResult, employeesResult] = await Promise.all([getWeeklyShifts(), getEmployees()]);
            const employees = employeesResult || [];
            const map: Record<string, Employee> = {};
            employees.forEach(e => (map[e.id] = e));
            setEmployeeMap(map);

            const monday = getCurrentWeekMonday(new Date());
            const filtered = (shiftsResult || []).filter(s => {
                const d = parseDateOnly(s.date);
                return d >= monday && d < addDays(monday, 7);
            });

            setShifts(filtered);
        } catch {
            setShifts([]);
            setEmployeeMap({});
        }
    }, []);

    useEffect(() => {
        let mounted = true;
        fetchData();
        const onCreated = () => {
            // re-fetch when a shift is created
            if (mounted) fetchData();
        };
        window.addEventListener("shiftCreated", onCreated);
        return () => {
            mounted = false;
            window.removeEventListener("shiftCreated", onCreated);
        };
    }, [fetchData]);

    return (
        <div className="centered-container">
            <section className="card roster-card">
                <h2>Weekly Roster</h2>

                <div className="table-wrap">
                    <table className="styled-table centered-table" role="table" aria-label="Weekly roster">
                        <thead>
                            <tr>
                                <th>Date</th>
                                <th>Employee</th>
                                <th className="numeric">Start</th>
                                <th className="numeric">End</th>
                            </tr>
                        </thead>
                        <tbody>
                            {shifts.map((s, idx) => {
                                const key = `${s.employeeId}-${s.date}-${s.startTime}-${idx}`;
                                const employee = employeeMap[s.employeeId];
                                const displayName = employee?.name ?? s.employeeId;
                                return (
                                    <tr key={key}>
                                        <td>{formatDate(s.date)}</td>
                                        <td>
                                            <div className="emp-info">
                                                <div className="emp-name">{displayName}</div>
                                            </div>
                                        </td>
                                        <td className="numeric">{formatTime(s.startTime)}</td>
                                        <td className="numeric">{formatTime(s.endTime)}</td>
                                    </tr>
                                );
                            })}
                        </tbody>
                    </table>
                </div>
            </section>
        </div>
    );
}

function parseDateOnly(dateOnly: string) {
    const parts = dateOnly?.split?.("-");
    if (!parts || parts.length < 3) return new Date(dateOnly);
    const [y, m, d] = parts.map(Number);
    return new Date(Date.UTC(y, m - 1, d));
}

function getCurrentWeekMonday(d: Date) {
    const local = new Date(d.getFullYear(), d.getMonth(), d.getDate());
    const day = local.getDay();
    const diff = (day + 6) % 7;
    local.setDate(local.getDate() - diff);
    return new Date(local.getFullYear(), local.getMonth(), local.getDate(), 0, 0, 0);
}

function addDays(d: Date, days: number) {
    const r = new Date(d);
    r.setDate(r.getDate() + days);
    return r;
}

function formatTime(iso?: string) {
    if (!iso) return "";
    try {
        const d = new Date(iso);
        return d.toLocaleTimeString([], { hour: "2-digit", minute: "2-digit" });
    } catch {
        return iso;
    }
}

function formatDate(dateStr: string) {
    try {
        const d = parseDateOnly(dateStr);
        return d.toLocaleDateString();
    } catch {
        return dateStr;
    }
}