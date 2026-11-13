export interface Receipt {
    orderId: number;
    receiptNumber: string;
    carMake: string;
    packageDetails: string;
    addOnDetails: string;
    totalAmount: number;
    createdAt: Date;
}
