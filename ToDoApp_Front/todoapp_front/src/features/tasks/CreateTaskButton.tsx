import React, { useState } from 'react';
import { Modal, Button, Form, Input, DatePicker, Select } from 'antd';
import { useDispatch } from 'react-redux';
import type { AppDispatch } from '../../app/store';
import { createTask } from './tasksSlice';


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
                    <Form.Item name="title" label="Назва" rules={[{ required: true }]}>
                        <Input />
                    </Form.Item>
                    <Form.Item name="description" label="Зміст" rules={[{ required: true }]}>
                        <TextArea rows={3} />
                    </Form.Item>
                    <Form.Item name="status" label="Статус" rules={[{ required: true }]}>
                        <Select
                            options={[
                                { value: 'ToDo', label: 'ToDo' },
                                { value: 'InProgress', label: 'InProgress' },
                                { value: 'Done', label: 'Done' }
                            ]}
                        />
                    </Form.Item>
                    <Form.Item name="deadline" label="Дедлайн" rules={[{ required: true }]}>
                        <DatePicker format="DD.MM.YYYY" style={{ width: '100%' }} />
                    </Form.Item>
                </Form>
            </Modal>
        </>
    );
}
export default CreateTaskButton;
