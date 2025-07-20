import { createSlice, createAsyncThunk} from '@reduxjs/toolkit';
import axios from 'axios';

export interface Task
{
    id: number;
    title: string;
    description: string;
    status: 'ToDo' | 'InProgress' | 'Done';
    deadline: string;
};
interface TaskState {
    tasks: Task[];
    loading: boolean;
    error: string | null;
};
const initialState: TaskState =
{
    tasks: [],
    loading: false,
    error: null
};
export const fetchTasks = createAsyncThunk<Task[]>(
    'tasks/fetchTasks',
    async (_, thunkAPI) => {
        try {
            const response = await axios.get('https://localhost:7233/Tasks');
            return response.data as Task[];
        }
        catch (error: unknown) {
            if (axios.isAxiosError(error)) {
                return thunkAPI.rejectWithValue(error.response?.data || error.message);
        }
            return thunkAPI.rejectWithValue("Невідома помилка");
        }
    }
);
export const createTask = createAsyncThunk<Task, Task>('tasks/createTask', async (task, thunkAPI) => {
    try {
        const response = await axios.post('https://localhost:7233/Tasks', task);
        return response.data;
    }
    catch (error: unknown) {
        if (axios.isAxiosError(error)) {
            return thunkAPI.rejectWithValue(error.response?.data || error.message);
        }
        return thunkAPI.rejectWithValue("Невідома помилка");
    }
});
export const patchTask = createAsyncThunk<Task, Task>('tasks/patchTask', async (task, thunkAPI) => {
    try {
        const response = await axios.patch(`https://localhost:7233/Tasks/${task.id}`, task);
        return response.data;
    }
    catch (error: unknown) {
        if (axios.isAxiosError(error)) {
            return thunkAPI.rejectWithValue(error.response?.data || error.message);
        }
        return thunkAPI.rejectWithValue("Невідома помилка");
    }
});

export const deleteTask = createAsyncThunk<number, number>(
    'tasks/deleteTask',
    async (id, thunkAPI) => {
        try {
            await axios.delete(`https://localhost:7233/Tasks/${id}`);
            return id;
        } catch (error: unknown) {
            if (axios.isAxiosError(error)) {
                return thunkAPI.rejectWithValue(error.response?.data || error.message);
            }
            return thunkAPI.rejectWithValue("Невідома помилка");
        }
    }
);
const tasksSlice = createSlice({
    name: "tasks",
    initialState,
    reducers:
    {

    },
    extraReducers: builder => {
        builder
            .addCase(fetchTasks.pending, state => {
                state.loading = true;
                state.error = null;
            })
            .addCase(fetchTasks.fulfilled, (state, action) => {
                state.loading = false;
                state.tasks = action.payload;
            })
            .addCase(fetchTasks.rejected, (state, action) => {
                state.loading = false;
                state.error = action.payload as string;
            })
            .addCase(createTask.fulfilled, (state, action) => {
                state.tasks.push(action.payload);
            })
            .addCase(patchTask.fulfilled, (state, action) => {
                const index = state.tasks.findIndex(t => t.id === action.payload.id);
                if (index !== -1) {
                    state.tasks[index] = action.payload;
                }
            })
            .addCase(deleteTask.fulfilled, (state, action) => {
                state.tasks = state.tasks.filter(t => t.id !== action.payload);
            });
    }
});
export default tasksSlice.reducer;
