import React from "react";
import { Link } from "react-router-dom";
import _ from "lodash";

const Pagination = ({ totalRecords, pageSize, onPageChange, selectedPage }) => {
  const pagesCount = Math.ceil(totalRecords / pageSize);

  if (pagesCount === 1) return null;

  const pages = _.range(1, pagesCount + 1);

  return (
    <React.Fragment>
      <nav aria-label="Page navigation example">
        <ul className="pagination flex-wrap">
          <li className="page-item">
            <a className="page-link" href="#" aria-label="Previous">
              <span aria-hidden="true">&laquo;</span>
            </a>
          </li>

          {pages.map((page) => {
            return (
              <li key={page} className="page-item">
                <Link
                  to="#"
                  onClick={() =>
                    onPageChange({ value: page, element: "pagination" })
                  }
                  className={
                    page === selectedPage
                      ? "page-link selcted-page-link"
                      : "page-link"
                  }
                >
                  {page}
                </Link>
              </li>
            );
          })}

          <li className="page-item">
            <a className="page-link" href="#" aria-label="Next">
              <span aria-hidden="true">&raquo;</span>
            </a>
          </li>
        </ul>
      </nav>
    </React.Fragment>
  );
};

export default Pagination;
