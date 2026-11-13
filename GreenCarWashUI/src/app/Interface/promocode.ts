export interface PromoCode {
    promoCodeId?: number;
  code: string;
  description: string;
  discountPercentage: number;
  expiryDate: string;
  isActive: boolean;
}
