export interface EmployeeSummary {
    employeeId: string;
    name: string;
    hoursWorked: number;
    tipAmount: number;
}

export interface WeeklySummary {
    totalTips: number;
    employees: EmployeeSummary[];
}

export interface Shift {
    employeeId: string;
    date: string;
    startTime: string;
    endTime: string;
}

export type CreateShiftRequest = {
    employeeId: string;
    date: string;
    startTime: string;
    endTime: string;
};

export interface Employee {
    id: string;
    name: string;
}