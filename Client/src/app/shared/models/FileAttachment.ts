export interface FileAttachment {
  fileAttachmentId: string;
  fileAttachmentCode: string;
  fileAttachmentName: string;
  fileExtention: string;
  fileType: string;
  size: number;
  fileUrl: string;
  latitude: string;
  longitude: string;
  createTime?: Date;
}
