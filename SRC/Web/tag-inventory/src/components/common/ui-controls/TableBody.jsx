import React from "react";
import TableFilter from "./TableFilter";

const TableBody = ({ columns, data, onTableFilter, searchVisible }) => {
  let itemCounter = 1;
  const renderCell = (item, column) => {
    if (column.content) {
      return column.content(item[column.path], item);
    }

    if (column.type === "calender") {
      const dt = new Date(Date.parse(item[column.path]));
      return dt.toLocaleDateString();
    }

    return item[column.path];
  };

  return (
    <React.Fragment>
      <tbody>
        {searchVisible && (
          <TableFilter
            columns={columns}
            onTableFilter={onTableFilter}
          ></TableFilter>
        )}
        {data.map(item => {
          return (
            <tr key={itemCounter++} style={{ textAlign: "center", height: 38 }}>
              {columns.map(column => {
                return (
                  <td
                    key={column.key}
                    style={{
                      paddingBottom: 0,
                      paddingRight: 0,
                      paddingTop: 0,
                      paddingLeft: 0,
                      verticalAlign: "center"
                    }}
                    className={
                      column.className
                        ? column.className(column.path, item[column.path])
                        : ""
                    }
                  >
                    {renderCell(item, column)}
                  </td>
                );
              })}
            </tr>
          );
        })}
      </tbody>
    </React.Fragment>
  );
};

export default TableBody;
