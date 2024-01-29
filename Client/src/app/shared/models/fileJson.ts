export interface FileDetails {
  name: string;
  type: string;
  size: number;
  content?: string; // Add this property if you want to include file content
}

export function fileToJson(file: File): FileDetails {
  const fileDetails: FileDetails = {
    name: file.name,
    type: file.type,
    size: file.size,
  };

  // Optionally, you can include the file content (e.g., for text files)
  const reader = new FileReader();
  reader.onload = (event) => {
    fileDetails.content = event.target?.result as string;
  };

  reader.readAsText(file);

  return fileDetails;
}
