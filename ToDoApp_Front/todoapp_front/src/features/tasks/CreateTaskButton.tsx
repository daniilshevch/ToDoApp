import React, { useState } from 'react';
import { Modal, Button, Form, Input, DatePicker, Select } from 'antd';
import { useDispatch } from 'react-redux';
import type { AppDispatch } from '../../app/store';
import { createTask } from './tasksSlice';
import dayjs from 'dayjs';

const { TextArea } = Input;
const CreateTaskButton: React.FC = () => {
    const [open, setOpen] = useState(false);
    const [form] = Form.useForm();
    const dispatch = useDispatch<AppDispatch>();

    const showModal = () => setOpen(true);
    const handleCancel = () => setOpen(false);

    const handleCreate = () => {
        form.validateFields().then(values => {
            const newTask =
            {
                id: Date.now(),
                title: values.title,
                description: values.description,
                status: values.status,
                deadline: values.deadline.toDate()
            };
            dispatch(createTask(newTask));
            setOpen(false);
            form.resetFields();
        });
      
    };

    return (
        <>
            <Button type="primary" onClick={showModal}>
                + Створити задачу
            </Button>
            <Modal
                title="Нова задача"
                open={open}
                onOk={handleCreate}
                onCancel={handleCancel}
                okText="Створити"
                cancelText="Скасувати"
            >
                <Form form={form} layout="vertical">
                    <Form.Item name="title" label="Назва" rules={[
                        { required: true, message: 'Назва є обов\'язковою' },
                        { max: 50, message: 'Максимальна довжина назви 50 символів' }]}>
                        <Input />
                    </Form.Item>
                    <Form.Item name="description" label="Зміст" rules={[
                        { required: true, message: 'Опис є обов\'язковим' },
                        { max: 500, message: 'Максимальна довжина опису 500 символів' }]}>
                        <TextArea rows={3} />
                    </Form.Item>
                    <Form.Item name="status" label="Статус" rules={[{ required: true, message:  'Статус є обов\'язковим' }]}>
                        <Select
                            options={[
                                { value: 'ToDo', label: 'До виконання' },
                                { value: 'InProgress', label: 'В процесі виконання' },
                                { value: 'Done', label: 'Зроблено' }
                            ]}
                        />
                    </Form.Item>
                    <Form.Item name="deadline" label="Дедлайн" rules={[{ required: true, message: 'Дедлайн є обов\'язковим' }]}>
                        <DatePicker format="DD.MM.YYYY" style={{ width: '100%' }} disabledDate={(current) => {
                            return current && current < dayjs().startOf('day');
                        }} />
                    </Form.Item>
                </Form>
            </Modal>
        </>
    );
}
export default CreateTaskButton;
