import React, { useRef } from "react";
import { useState } from "react";
import { useLocation, useHistory } from "react-router-dom";
import InputMask from "react-input-mask";

const TableFilter = (props) => {
  const check = useRef(false);

  let itemCounter = 1;
  const { columns, onTableFilter } = props;

  const location = useLocation();
  const history = useHistory();

  const renderFilterCell = (column) => {
    if (column.type === "calender") {
      return (
        <InputMask
          mask="99/99/9999"
          placeholder="mm/dd/yyyy"
          className="form-control"
          style={{ textAlign: "center", fontSize: 10 }}
          onKeyUp={(e) =>
            onTableFilter({
              url: location.pathname,
              column: column.path,
              value: e.target.value === "__/__/____" ? "" : e.target.value,
              type: "calender",
              element: "tablefilter",
            })
          }
        />
      );
    }

    return column.path === "checkBox" ? (
      <input
        type="checkbox"
        checked={check.current}
        onClick={(e) => {
          check.current = !check.current;
          onTableFilter({
            url: location.pathname,
            column: column.path,
            value: check.current,
            element: "tablefilter",
          });
        }}
      ></input>
    ) : (
      <input
        className="form-control"
        type="text"
        style={{ textAlign: "center", fontSize: 10 }}
        name={column.path}
        disabled={column.path === "checkBox"}
        onKeyUp={(e) =>
          onTableFilter({
            url: location.pathname,
            column: column.path,
            value: e.target.value,
            element: "tablefilter",
          })
        }
      ></input>
    );
  };

  return (
    <React.Fragment>
      <tr key={itemCounter++} style={{ textAlign: "center", fontSize: 10 }}>
        {columns.map((column) => {
          return <td key={itemCounter++}>{renderFilterCell(column)}</td>;
        })}
      </tr>
    </React.Fragment>
  );
};

export default TableFilter;
