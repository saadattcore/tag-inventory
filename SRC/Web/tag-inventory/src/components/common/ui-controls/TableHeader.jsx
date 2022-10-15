import React from "react";

const TableHeader = ({ columns }) => {
  return (
    <React.Fragment>
      <thead className="table-header">
        <tr>
          {columns.map((column) => {
            return (
              <th className="table-header-row" key={column.key}>
                {column.title}
              </th>
            );
          })}
        </tr>
      </thead>
    </React.Fragment>
  );
};

export default TableHeader;
