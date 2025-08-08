export interface Category {
    id: number;
    name: string;
}

export interface Task {
    id: number;
    title: string;
    dueDate?: string | null;
    completedDate?: string | null;
    categoryId?: number | null;
}

export interface TaskState {
    loading: boolean;
    error: string | boolean | null;
    activeTasks: Task[];
    completedTasks: Task[];
    categories: Category[];
}