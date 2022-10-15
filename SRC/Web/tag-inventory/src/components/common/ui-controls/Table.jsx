import React, { Component } from "react";
import TableHeader from "./TableHeader";
import TableBody from "./TableBody";

const Table = (props) => {
  return (
    <React.Fragment>
      <table className="table table-hover table-sm component-table">
        <TableHeader columns={props.columns}></TableHeader>
        <TableBody
          columns={props.columns}
          data={props.data}
          onTableFilter={props.onTableFilter}
          searchVisible={props.searchVisible}
        ></TableBody>
      </table>
    </React.Fragment>
  );
};

export default Table;
