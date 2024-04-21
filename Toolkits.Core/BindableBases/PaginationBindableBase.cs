using System.Diagnostics;

namespace Toolkits.Core;

/// <summary>
///  create new instance of the <see cref="PaginationBindableBase"/>
/// </summary>
public abstract class PaginationBindableBase : BindableBase
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string oldSearchCondition = string.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="PaginationBindableBase"/> class.
    /// </summary>
    protected PaginationBindableBase()
    {
        NextPageCommand = new RelayCommandAsync(OnNextPageCommandAsync);
        LastPageCommand = new RelayCommandAsync(OnLastPageCommandAsync);
        PreviousPageCommand = new RelayCommandAsync(OnPreviousPageCommandAsync);
        FirstPageCommand = new RelayCommandAsync(OnFirstPageCommandAsync);
        GotoCommand = new RelayCommandAsync(OnGotoCommandAsync);
        SearchCommand = new RelayCommandAsync(OnSearchCommandAsync);
    }

    /// <summary>
    /// total page
    /// </summary>
    public virtual int TotalPage
    {
        get => GetValue(1);
        set => SetValue(value.FromRange(1, int.MaxValue));
    }

    /// <summary>
    /// current page
    /// </summary>
    public virtual int CurrentPage
    {
        get => GetValue(1);
        set => SetValue(value.FromRange(1, int.MaxValue));
    }

    /// <summary>
    /// the index of the jump page
    /// </summary>
    public virtual int TargetPage
    {
        get => GetValue(1);
        set => SetValue(value.FromRange(1, int.MaxValue));
    }

    /// <summary>
    /// the count of per page
    /// </summary>
    public virtual int PageSize
    {
        get => GetValue(10);
        set => SetValue(value.FromRange(1, int.MaxValue));
    }

    /// <summary>
    /// the count of per page
    /// </summary>
    public virtual int TotalCount
    {
        get => GetValue(0);
        set => SetValue(value.FromRange(0, int.MaxValue));
    }

    /// <summary>
    /// Keyword
    /// </summary>
    public virtual string Keyword
    {
        get => GetValue(string.Empty);
        set => SetValue(value);
    }

    /// <summary>
    /// IsSearching
    /// </summary>
    public virtual bool IsSearching
    {
        get => GetValue(false);
        set => SetValue(value);
    }

    /// <summary>
    /// NextPageCommand
    /// </summary>
    public virtual RelayCommandAsync NextPageCommand { get; }

    /// <summary>
    /// LastPageCommand
    /// </summary>
    public virtual RelayCommandAsync LastPageCommand { get; }

    /// <summary>
    /// PreviousPageCommand
    /// </summary>
    public virtual RelayCommandAsync PreviousPageCommand { get; }

    /// <summary>
    /// FirstPageCommand
    /// </summary>
    public virtual RelayCommandAsync FirstPageCommand { get; }

    /// <summary>
    /// GotoCommand
    /// </summary>
    public virtual RelayCommandAsync GotoCommand { get; }

    /// <summary>
    /// SearchCommand
    /// </summary>
    public virtual RelayCommandAsync SearchCommand { get; }

    private async Task OnSearchCommandAsync()
    {
        try
        {
            if (IsSearching)
            {
                return;
            }

            IsSearching = true;

            string currentSearchCondition = Keyword ??= string.Empty;

            if (oldSearchCondition != currentSearchCondition)
            {
                CurrentPage = 1;
            }

            TotalCount = await SearchAsync(currentSearchCondition, CurrentPage, PageSize);

            TotalPage = (int)Math.Ceiling(TotalCount / (double)PageSize);

            oldSearchCondition = currentSearchCondition;
        }
        finally
        {
            IsSearching = false;
        }
    }

    private async Task OnGotoCommandAsync()
    {
        if (TargetPage < 1 || TargetPage > TotalPage || CurrentPage == TargetPage)
        {
            return;
        }

        CurrentPage = TargetPage;

        await SearchAsync(Keyword, CurrentPage, PageSize);
    }

    private async Task OnFirstPageCommandAsync()
    {
        if (CurrentPage == 1)
        {
            return;
        }
        CurrentPage = 1;
        await SearchCommand.ExecuteAsync();
    }

    private async Task OnPreviousPageCommandAsync()
    {
        int currentPage = CurrentPage - 1;

        if (currentPage < 1)
        {
            return;
        }

        CurrentPage = currentPage;
        await SearchCommand.ExecuteAsync();
    }

    private async Task OnLastPageCommandAsync()
    {
        if (CurrentPage == TotalPage)
        {
            return;
        }
        CurrentPage = TotalPage;
        await SearchCommand.ExecuteAsync();
    }

    private async Task OnNextPageCommandAsync()
    {
        int currentPage = CurrentPage + 1;

        if (currentPage > TotalPage)
        {
            return;
        }

        CurrentPage = currentPage;
        await SearchCommand.ExecuteAsync();
    }

    /// <summary>
    ///     <para>keyword:keyword of currentSearchCondition</para>
    ///     <para>currentPage:the number of page</para>
    ///     <para>pageSize:max count in a page</para>
    ///     <para>returns:the number of total count</para>
    /// </summary>
    /// <param name="keyword">keyword of currentSearchCondition</param>
    /// <param name="currentPage">the number of page</param>
    /// <param name="pageSize">the count in a page</param>
    /// <returns>total count</returns>
    protected abstract Task<int> SearchAsync(string keyword, int currentPage, int pageSize);
}
