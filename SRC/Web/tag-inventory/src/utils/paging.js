function paginate(pageConfig) {
  const { pageSize, pageNumber } = pageConfig;

  const startIndex = (pageNumber - 1) * pageSize;
  const endIndex = pageNumber * pageSize - 1;

  return { startIndex: startIndex, endIndex: endIndex };
}

export default paginate;
