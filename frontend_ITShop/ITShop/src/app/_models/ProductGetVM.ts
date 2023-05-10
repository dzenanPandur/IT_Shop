import {ProductPictureGetVM} from "./ProductPictureGetVM";

export interface ProductGetVM{
  id: number;
  name: string;
  categoryID: number;
  productPictures: Array<ProductPictureGetVM>;

}
